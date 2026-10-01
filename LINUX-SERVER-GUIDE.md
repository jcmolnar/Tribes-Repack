# Hosting a Modern Tribes server on Linux

`tribes-server` runs a Modern Tribes dedicated server on a Linux machine or VPS with
**no desktop, no game window and no game client**. One command installs the server,
one command updates it, and it keeps the server running: crashed or frozen servers are
restarted, and a server that keeps crashing is stopped and reported instead of looping
forever.

On Windows, use **TribesHost.exe** instead (it is in your game folder). Both write the
same game settings; this guide is only about Linux.

---

## Contents

1. [What you need](#1-what-you-need)
2. [Install Wine](#2-install-wine)
3. [Install the server](#3-install-the-server)
4. [Set it up: server.ini](#4-set-it-up-serverini)
5. [Pick a mod and a map](#5-pick-a-mod-and-a-map)
6. [Run it](#6-run-it)
7. [Run it as a service (starts at boot)](#7-run-it-as-a-service-starts-at-boot)
8. [Control screen, console, logs and admin](#8-control-screen-console-logs-and-admin)
9. [Updates](#9-updates)
10. [Asset Store mods (Red Moon, Annihilation, Star Wars, ...)](#10-asset-store-mods)
11. [Ports, firewall and letting players in](#11-ports-firewall-and-letting-players-in)
12. [Bots](#12-bots)
13. [More than one server on one machine](#13-more-than-one-server-on-one-machine)
14. [Presets and backups](#14-presets-and-backups)
15. [Troubleshooting](#15-troubleshooting)
16. [Command reference](#16-command-reference)
17. [Where everything lives](#17-where-everything-lives)
18. [Uninstall](#18-uninstall)

---

## 1. What you need

| | |
|---|---|
| **OS** | 64-bit Linux. Tested on **Ubuntu 22.04** with Wine 6.0.3. Other distros should work if they can run 32-bit Wine, but are untested. |
| **Software** | Python 3.8 or newer, and Wine with **32-bit** support. Nothing else -- no desktop, no X server, no graphics drivers, no sound. |
| **Disk** | About **4.5 GB** for the server (base game + bundled mods), plus ~0.5 GB for Wine's files. Asset Store mods add 2 MB to 1.1 GB each. |
| **Memory** | ~250 MB for a server with a full bot game (measured). 1 GB of RAM total is comfortable. |
| **CPU** | Light. One core is plenty for a normal server. |
| **Network** | UDP port 28001 (or the one you choose) and that port + 2 for voice chat, reachable by players. |

The server is the same `ModernTribes.exe` Windows players use, run through Wine. There is
nothing to compile.

## 2. Install Wine

You need Wine **with 32-bit support**.

**Ubuntu / Debian**

```bash
sudo dpkg --add-architecture i386
sudo apt update
sudo apt install --no-install-recommends python3 wine wine32:i386
```

`--no-install-recommends` keeps it small: the server needs none of the graphics or audio
extras Wine normally pulls in.

**Other distros** (untested): install your distribution's `wine` package with its 32-bit
(multilib) parts, plus `python3`.

Check it worked:

```bash
wine --version
python3 --version
```

## 3. Install the server

Download the installer and install into a folder of your choice (here `~/tribes`):

```bash
wget https://raw.githubusercontent.com/jcmolnar/Tribes-Repack/main/tribes-server
python3 tribes-server install ~/tribes
```

This downloads the **server-only** set: everything a server needs, without the
client-only HD textures, player skins, videos, music, sky art and fonts (about 4 GB
instead of 7.5 GB). Every file is checked against its published SHA-256 hash. If the
download is interrupted, run the same command again -- finished files are kept.

Options:

| Option | What it does |
|---|---|
| `--full` | Download everything the game client gets (7.5 GB). Only needed if you also want to run the client from this folder. |
| `--channel nightly` | Follow a different update channel (only if you have been told to). |

Then check everything is in order:

```bash
cd ~/tribes
./tribes-server doctor
```

`doctor` checks Python, Wine, the game files, free disk space, and that your ports are free.

## 4. Set it up: server.ini

The first install writes `server.ini` in the install folder. Edit it with any text editor
(`nano server.ini`), or change one value at a time:

```bash
./tribes-server config set server.name "My Tribes Server"
./tribes-server config            # show what will be used
```

Changes take effect the next time the server starts (`./tribes-server restart`).

`server.ini` holds your passwords, so it is created readable only by you.

### [server] -- the game

| Key | Default | Meaning |
|---|---|---|
| `profile` | `base` | Which mod to host. `./tribes-server mods` lists them (see [section 5](#5-pick-a-mod-and-a-map)). |
| `mods` | *(blank)* | Advanced: override the `-mod` launch words, e.g. `rpg rmrpg`. A word starting with `+` is launched but not advertised to players (a mod built on another mod advertises only the parent). Blank = the profile's own. |
| `name` | `TRIBES Server` | Name shown in the server browser. |
| `mission` | *(blank)* | Starting map. Blank = the profile's default (base `raindance`, Tribes RPG `rpgmap6`, Red Moon `RMR`, Mech Mayhem `MechArena1`); for other mods, the last map saved in `config/ServerPrefs.cs`, or the mod's first map. |
| `port` | *(blank)* | UDP game port. Blank = the profile's default (28001; Red Moon 28002; Mech Mayhem 28004). Voice chat uses this port **+ 2**. |
| `max_players` | `8` | 1 to 128. |
| `join_password` | *(blank)* | Password players need to join. Blank = open server. |
| `admin_password` | *(blank)* | Admin login password. **Set one** before going public. For Tribes RPG / Red Moon it fills all five admin slots. |
| `public` | `false` | `true` = announce on the public master server list. `false` = players join by address only. |
| `info` | *(blank)* | Server info text in the browser. Use `\n` for a new line. No double quotes. |
| `motd` | *(blank)* | Message shown to players on join (Tribes RPG uses it as the login message). |
| `rotation` | *(blank)* | Custom map cycle, comma separated, e.g. `Raindance, Broadside, DangerousCrossing`. Needs 2 or more maps. |
| `bots` | `false` | `true` / `false`: BotBrain bots for base Tribes. See [Bots](#12-bots). |
| `bots_team0`, `bots_team1` | *(blank)* | Bot cap for team 1 / team 2. Blank = full roster. |
| `mixed_types` | `false` | Allow mixing game types in one rotation. |
| `voice_chat` | `true` | In-game voice relay (uses port + 2). |
| `netcode` | `legacy` | `legacy` = every client can join. `modern150` = Modern Tribes 1.50 clients only, and enables the four options below. |
| `lag_comp` | `false` | (modern150) Hits are checked against what the shooter saw. |
| `fire_from_view` | `false` | (modern150) Shots leave from where the shooter's screen shows them. |
| `require_current_client` | `false` | (modern150) Refuse 1.50 clients older than this server's version. |
| `cheat_observe` | `true` | Log-only warnings that name a player worth watching. Never kicks anyone. |
| `rpg_limits` | *(blank)* | (modern150) Raised content limits for RPG mods. Blank = on automatically for modern150 + an RPG mod. |
| `competitive` | `false` | One switch for competitive play: forces modern150, lag compensation, current client and cheat observation on. |

### [supervisor] -- keeping it running

| Key | Default | Meaning |
|---|---|---|
| `restart_delay` | `5` | Seconds to wait before restarting a server that exited. |
| `crash_loop_restarts` | `5` | Stop restarting after this many exits in a row that each happened within a minute of starting. The server then stays down until you fix it -- see [Troubleshooting](#15-troubleshooting). |
| `hang_timeout` | `180` | Restart a server that stops answering status queries for this many seconds (after it has answered once). `0` = off. |

### [update]

| Key | Default | Meaning |
|---|---|---|
| `auto_update` | `false` | `true` = check for updates every `check_minutes` and install them **only while nobody is playing**, then restart. |
| `check_minutes` | `30` | How often auto-update checks (minimum 5). |
| `mode` | `server` | `server` = skip client-only content. `full` = everything. |
| `exclude` | *(blank)* | Extra folders not to download, comma separated, e.g. `config/nav/` (bot navigation, 1.7 GB -- only if you never run bots) or `Mods/War40k/`. |
| `manifest_url` | *(blank)* | Leave blank (official updates). |
| `store_url` | *(blank)* | Leave blank (official Asset Store). `0` = never contact the store. |

### [network]

| Key | Default | Meaning |
|---|---|---|
| `probe_url` | *(blank)* | Optional Internet port-test service (the same one TribesHost can use). Leave blank unless you run one. |

### [wine]

| Key | Default | Meaning |
|---|---|---|
| `wine` | `wine` | The Wine command. |
| `prefix` | *(blank)* | Wine's settings folder. Blank = `.tribes-server/wine` inside the install, created on first start. |
| `arch` | `win32` | Wine architecture for that folder. Leave it. |
| `debug` | `-all` | Wine's own log output. Leave it. |

## 5. Pick a mod and a map

```bash
./tribes-server mods                     # mods you can host (profiles)
./tribes-server config set server.profile rpg
./tribes-server missions                 # maps for that mod, with game type
./tribes-server missions --type capture  # only Capture the Flag maps (matches part of the type)
./tribes-server config set server.mission Broadside
```

Included in the download: **base Tribes, Tribes RPG, Mech Mayhem, War40k, SEX, TSC**
(plus small content folders) -- each of these was hosted and checked on Linux. More mods come from the Asset Store -- see
[section 10](#10-asset-store-mods).

Settings for **Tribes RPG** and **Red Moon RPG** also live in their own files,
`config/rpgserv.cs` and `config/rmrpgserv.cs` (the mods read those). `tribes-server`
fills in the name, port, players, passwords and map there for you, and leaves every
other RPG setting in those files alone -- edit them directly for RPG gameplay settings.

**Kingdom of Kronos cannot be hosted.** It is a live, persistent server. Hosting the
`rpg` profile gives you a plain Tribes RPG server, not a copy of Kronos.

## 6. Run it

**In the foreground** (stops when you close the terminal or press Ctrl-C):

```bash
./tribes-server run
```

**In the background:**

```bash
./tribes-server start
./tribes-server status
./tribes-server stop
./tribes-server restart       # restarts the game and re-reads server.ini
```

`status` shows whether it is running, players, current map, version, uptime and
restarts:

```
state     running (pid 2167, up 434s, 1 restart(s))
version   v30 installed, v30 latest, mode=server, auto-update on
server    My Tribes Server -- 3/16 players, Raindance (CTF), port 28001
```

The very first start also sets up Wine's folder (about 10 seconds on the test machine).
After that a server is up in 10-15 seconds.

For a server that should stay up for good, use a service instead (next section).

**Prefer a screen to commands?** `./tribes-server tui` does all of the above (and more)
from one full-screen view -- see [section 8](#8-control-screen-console-logs-and-admin).

## 7. Run it as a service (starts at boot)

```bash
./tribes-server systemd | sudo tee /etc/systemd/system/tribes-server.service
sudo systemctl daemon-reload
sudo systemctl enable --now tribes-server
```

Then:

```bash
sudo systemctl status tribes-server
sudo systemctl restart tribes-server
sudo systemctl stop tribes-server
journalctl -u tribes-server -f          # live server output
./tribes-server status                   # still works
./tribes-server console                  # still works
```

The service runs as the user who generated it. It restarts the supervisor if that ever
dies, but **not** after a crash loop or a broken `server.ini` (those exit with code 3 so
a broken server stays down until someone looks).

Do not use `./tribes-server start` and the service at the same time for one install.

## 8. Control screen, console, logs and admin

### The control screen (`tui`)

```bash
./tribes-server tui
```

A full-screen control panel in the terminal -- over SSH too, nothing extra to install
(it uses Python's built-in `curses`). Closing it **never** stops the server, it works
alongside the service from section 7, and several people can have it open at once.

```
 My Tribes Server   RUNNING  up 2h14m  restarts 0
 Raindance (CTF)  |  3/16 players  |  port 28001  |  v30  |  auto-update on
 F2 Console   F3 Players   F4 Maps   F5 Settings   F6 Store   F1 Help
 ...the live server console...
 >  type a command here
 F7 Stop  F8 Restart  F9 Update  F10 Quit  Tab Next screen  F1 Help
```

| Screen | What you do there |
|---|---|
| **F2 Console** | The live server console. Type a command, Enter runs it. Up/Down = command history (kept between sessions), PgUp/PgDn scroll back, End or Esc = back to live. |
| **F3 Players** | Who is on: client id, team, address, name. Enter on a player = **kick** or **ban for 30 minutes**. `m` = message everyone, `r` = refresh. Bots are not listed. |
| **F4 Maps** | Every map for the current mod, with its game type. Type to filter by part of a name or a type -- or a type's initials (`ctf` finds every Capture the Flag map). Enter = **switch to it now**, make it the **starting map**, or **add it to the rotation**. `*` marks the map being played. |
| **F5 Settings** | Everything in `server.ini`'s `[server]` section plus auto-update and the restart rules, with the explanation of the selected one underneath. Enter changes it: on/off settings flip, the mod and the map are picked from a list, text is typed in. Saved straight to `server.ini` (your comments are kept); **F8** applies it to a running server. Changing the mod resets the map to that mod's default. |
| **F6 Store** | The Asset Store: what is installed, what has an update, download sizes. Enter = install or update, `x` = remove (server stopped). After installing a mod it offers to host it. |
| **F1 Help** | Keys, and the admin command cheat sheet. |

Keys that work everywhere: **F7** start / stop, **F8** restart, **F9** update (asks:
when the server is empty, or now), **F10** or **Ctrl-Q** quit, **Tab** next screen,
**Ctrl-L** repaint. Stop, restart, kick, ban and switching maps with players on always
ask first (`y` to confirm). Long jobs (start, update, store install) show their progress
in the console screen.

The terminal needs to be at least 60 x 12 characters. If F1-F4 do nothing in your
terminal, use Tab to move between screens.

### Console, logs and admin commands

**Interactive console** -- type any server console command; you see the server's output
live. Ctrl-D or Ctrl-C leaves the console; **the server keeps running**:

```bash
./tribes-server console
```

**One command**, then show the output for a moment:

```bash
./tribes-server console 'messageAll(0, "Server restarting in 5 minutes");'
```

**Logs:**

```bash
./tribes-server logs           # last 50 lines of console_server.log
./tribes-server logs -f        # follow it live
./tribes-server logs -n 500
```

**Admin commands:**

```bash
./tribes-server admin
```

lists the commands for the server console (no login needed there) and for players'
in-game console (they log in with `AdminPassword("your password");`). To kick or ban, use
the Players screen in `tui`, or the in-game admin menu.

## 9. Updates

```bash
./tribes-server update --check      # is there an update? (exit code 10 = yes)
./tribes-server update              # install it
```

`update` downloads only the files that changed and verifies every one.

- **Server stopped:** it updates right away.
- **Server running:** it waits until **nobody is playing**, stops the server, updates and
  starts it again. Nobody gets kicked. Use `./tribes-server update --now` to update
  immediately anyway (players are disconnected).
- **Automatic:** set `auto_update = true` in `server.ini`. The server checks every
  `check_minutes` and updates itself whenever it is empty. It also checks once at start.

Installed Asset Store mods are updated by the same command.

**Never overwritten by updates:** `server.ini`, `config/ServerPrefs.cs`, `config/rpgserv.cs`,
`config/rmrpgserv.cs`, `modlist.txt`, and your presets. The `tribes-server` script updates
itself as part of the normal update.

Other options: `update --verify` re-checks every file's hash and repairs anything damaged;
`update --full` adds the client-only content.

## 10. Asset Store mods

Mods that are not part of the main download -- **Red Moon RPG, Annihilation, Shifter,
Star Wars, Star Wars RPG, Tac, Herc Havoc, Duel, Delta Air Force, Reality Bites,
Starsiege** and more -- come from the Asset Store, the same one the game client uses.

```bash
./tribes-server store                     # list them, with download size and what you have
./tribes-server store install rmrpg       # install one (or several: install a b c)
./tribes-server config set server.profile rmrpg
./tribes-server restart
./tribes-server store remove rmrpg        # stop the server first
```

`install` tells you the profile name to host it with. Hosted and checked on Linux so far:
Red Moon RPG, Annihilation and Shifter; the others install the same way but have not
each been hosted on Linux yet. Packs listed under `assets` or
`skins` (models, HD textures, voices) only change how the game looks to players; a server
does not need them.

`remove` deletes only files the pack installed and that you have not changed; files
another pack or the game also uses are kept.

## 11. Ports, firewall and letting players in

The server uses **UDP**: the game port (28001 by default) and **game port + 2** for voice.

```bash
./tribes-server netcheck
```

shows your LAN and Internet join addresses, whether the server answers, and the exact
firewall command for your system. Typically:

- **ufw** (Ubuntu): `sudo ufw allow 28001:28003/udp`
- **firewalld** (Fedora/RHEL): `sudo firewall-cmd --permanent --add-port=28001-28003/udp && sudo firewall-cmd --reload`
- **Cloud VPS:** also open the UDP ports in the provider's firewall / security group.
- **Home connection:** forward the UDP ports on your router to this machine.

**How players join:**

- `public = true`: the server appears in the in-game server browser.
- Otherwise (or always): players join by address. In the game's console (`~`):
  `$Server::Address = "IP:203.0.113.5:28001"; JoinGame();` -- with your address from
  `netcheck`.

## 12. Bots

Base Tribes can fill teams with BotBrain bots that play real CTF. They are **off** unless
you turn them on.

```bash
./tribes-server config set server.bots true
./tribes-server config set server.bots_team0 4     # optional caps
./tribes-server config set server.bots_team1 4
./tribes-server restart
```

Bots join as soon as the map loads, before any human is on. They do not count as players
in `status`, and an empty server (no humans) still counts as empty for automatic updates.
They need the map's navigation graph (`config/nav/<map>.nav`, included for hundreds of
maps). Do not use
`exclude = config/nav/` if you want bots. The bot roster is in `config/botbrain.cfg`.

## 13. More than one server on one machine

Use **one install folder per server** (each is its own copy of the game), with
different ports:

```bash
python3 tribes-server install ~/tribes-ctf
python3 tribes-server install ~/tribes-rpg
cd ~/tribes-rpg && ./tribes-server config set server.port 28010
```

Leave a gap of at least 3 between ports (each server also uses port + 2 for voice). For
services, give each one a name:

```bash
cd ~/tribes-rpg && ./tribes-server systemd --name rpg | sudo tee /etc/systemd/system/tribes-server-rpg.service
```

## 14. Presets and backups

Save and switch between whole setups (like TribesHost's presets). **Passwords are never
saved in a preset**, and loading one keeps your current passwords.

```bash
./tribes-server config save "ctf night"
./tribes-server config presets
./tribes-server config load "ctf night"
./tribes-server restart
```

The first time `tribes-server` edits `config/rpgserv.cs` or `config/rmrpgserv.cs` it keeps
the original as `*.hostgui.bak`. To put the originals back:

```bash
./tribes-server config restore-backup
```

## 15. Troubleshooting

Start with:

```bash
./tribes-server doctor
./tribes-server status
./tribes-server logs -n 200
```

| Symptom | Cause / fix |
|---|---|
| `'wine' not found` | Install Wine ([section 2](#2-install-wine)). |
| Server exits immediately, over and over; status says `CRASH LOOP` | Something stops it from starting. Read `./tribes-server logs` and `.tribes-server/supervisor.log` (or `journalctl -u tribes-server`). Likely causes: Wine without 32-bit support, a broken edit in `config/*.cs`, a mod's files missing. Fix it, then `./tribes-server start`. |
| `cannot start the server` / `unknown profile` | `server.ini` names a mod that is not installed. `./tribes-server mods` lists valid profiles. |
| `not answering status queries yet` for more than a minute | Look at `./tribes-server logs`. If the map failed to load, set `server.mission` to one from `./tribes-server missions`. |
| `Error: no mission provided` in the log | No map set and none found for that mod. Set `server.mission`. |
| `UDP 28001 is in use` | Another server (or program) uses the port. Change `server.port`, or stop the other one. |
| Players cannot connect | Run `./tribes-server netcheck`; open/forward the UDP ports ([section 11](#11-ports-firewall-and-letting-players-in)). |
| Server not in the browser | Set `public = true` and restart, and make sure the ports are reachable from the Internet (`netcheck`). |
| Bots do not join | `server.bots = true`, restart, and the map needs `config/nav/<map>.nav` (the log says `[BOTBRAIN] nav graph: ...` when it loads one). |
| `not enough disk space` | Free space, or skip folders with `[update] exclude`. |
| Update failed half way | Run `./tribes-server update` again; downloads resume and nothing was replaced until every file verified. |
| `native_crash_*.txt` files in the folder | Crash reports from the game. Send them along with `console_server.log` when reporting a problem. |
| Lots of `exec: invalid script file ...` / `MOUNT FAILED` lines in the log | Normal. Mods try to load optional files; Windows servers print the same lines. |

When asking for help, include the output of `./tribes-server doctor`,
`./tribes-server status` and the end of `console_server.log`.

## 16. Command reference

Run in the install folder (or add `--dir <folder>` before the command).

| Command | What it does |
|---|---|
| `install <folder> [--full] [--channel X]` | First-time install into a folder. |
| `doctor` | Check Wine, Python, files, disk, ports. |
| `config` | Show the settings that will be used. |
| `config set section.key value` | Change one setting in `server.ini`. |
| `config save / load <name>`, `config presets` | Presets (no passwords stored). |
| `config restore-backup` | Restore the original RPG serv files. |
| `mods` | Hostable profiles. |
| `missions [--type X]` | Maps for the current profile, with game type. |
| `store`, `store install <id...>`, `store remove <id...>` | Asset Store mods. |
| `run [--update] [--no-update]` | Run in the foreground. |
| `start`, `stop`, `restart` | Run in the background / stop / restart (re-reads `server.ini`). |
| `status [--json]` | State, players, map, version, uptime. Exit code 3 = not running. |
| `tui` | Full-screen control screen: console, players (kick/ban), maps, settings, store. |
| `console ['command']` | Interactive console, or run one command. |
| `logs [-f] [-n N]` | Show / follow `console_server.log`. |
| `admin` | Admin command cheat sheet. |
| `netcheck` | Join addresses, firewall and port-forward help. |
| `update [--check] [--now] [--verify] [--full]` | Update game files and installed store mods. |
| `systemd [--name X]` | Print a systemd service for this install. |

## 17. Where everything lives

Inside the install folder:

| Path | What |
|---|---|
| `server.ini` | Your settings (yours -- never overwritten). |
| `tribes-server` | This tool (updated with the game). |
| `ModernTribes.exe`, `base/`, `Mods/`, `config/` | The game. |
| `console_server.log` | The server's console log. |
| `config/hostgui.cs` | Written from `server.ini` on every start -- do not edit. |
| `config/ServerPrefs.cs`, `config/rpgserv.cs`, `config/rmrpgserv.cs` | Game server settings files (kept across updates). |
| `updpacks/` | Records of installed Asset Store mods. |
| `updver.txt` | Installed version. |
| `.tribes-server/` | Tool state: Wine folder (`wine/`), presets, `supervisor.log` (background runs), control socket, `tui_history` (control-screen command history, readable only by you). |
| `_update/` | Temporary download folder during updates. |

## 18. Uninstall

```bash
./tribes-server stop
sudo systemctl disable --now tribes-server      # if you set up the service
sudo rm /etc/systemd/system/tribes-server.service
rm -rf ~/tribes
```
