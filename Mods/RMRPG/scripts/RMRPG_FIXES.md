# Red Moon RPG — script bug-fix log

## FLAG DECISIONS (resolved with the user, applied in commits 711dbbd / bae013c / 68c5e01)
- #steal: FLIP — Rogues now get the self-level (better) formula; non-Rogues get the $LimitSteal penalty. (comchat.cs, added `== 0`.)
- Status/cage durations: FIX — changed `X += Cap(X+time,...)` to `X = Cap(...)` in all 4 Status.cs
  effects + carling1 cage, so durations respect the 10-min $MaxStatusTime cap. (Alcohol's
  `Alvl += %res` is intentional accumulation — left.)
- Smith material cost: FIX — `count * multiplier` bareword -> `%multiplier` (99-arrow batches
  now consume 99x materials). RETUNE pricing if needed.
- Carling PK-logging: LEAVE as-is (flagged only; still cages on every lowbie PK).
- carling1 drop typos: CONFIRMED typos (Pearl/Platinum/Emerald are the real item names; Pear/
  Platnium/Emeral appear only as bare drop-list words) — FIXED.
- comchat2 admin commands: FIX ALL — #setpl guard reordered (resolve %id first), #getexp uses
  %id consistently, #getpassword/#getotherinfo show the target's data, 4 set-cmds refreshall(%id).
- Drag_Sword/Punisher onFire: CONFIRMED misattached (traced MakeItem model-name derivation) —
  Drag_Sword branch moved into longstaff4Image::onFire; Punisher handler renamed to
  greensword125Image::onFire.
- Chocobo trade/breed menus: FIX — `if(%char == -1)` (never true) -> `if(%index >= 10)` loop cap.
- Chocobo jump-dismount: WIRE UP — new guarded `Armor::jump` (chocobo -> dismount, else
  Player::jump) + fixed `Armor::dismount` vars + Chocobo::Delete cleanup. NOTE: assumes the jump
  callback fires with %this = the controlled chocobo; needs an in-game test to confirm it doesn't
  disturb normal-player jumping (guarded to fall through to Player::jump, so it should be safe).
- LEFT as-is (user call): assassin bounty-timeout reversed $state key; DeusKeys dead code;
  ConvNum '.05' cosmetic; AI::SayZone self-send cosmetic.

---


Source: extracted from `scripts.vol` (packed 2015-10-17), the newer of the two
copies (the `Downloads/rmrpg` tree is an older 2002-2008 snapshot). This folder
is the git-tracked canonical source; deploy = loose scripts here with
`scripts.vol` made unreachable (loose files override the vol).

Baseline commit: `9e5adae` (as-extracted, pre-fix).

Status: `[x]` fixed · `[ ]` open · `[?]` needs investigation/decision

---

## FIXED

### 1. Chocobo.cs — leftover "TEMP" Cap()/round() clobbered rpgfunk's (HIGH impact)
- [x] Chocobo.cs had `////TEMP` `function Cap()` + `function round()` redefinitions.
  Chocobo loads AFTER rpgfunk (Server.cs: rpgfunk L114, Chocobo L163), so these
  won overrode the real ones. This Cap() dropped rpgfunk's `"inf"` (uncapped)
  branch, so `Cap(x, lb, "inf")` compared `x > "inf"` (→ x>0) and returned the
  STRING "inf". That silently broke, mod-wide:
    - playerdamage.cs `Cap(%value-25, 0, "inf")` — damage math
    - rpgstats.cs `pow(Cap(%i-20, 0, "inf"), 5)` — EXP table generation
    - blackSmith.cs `Cap(round(... *0.75), 1, "inf")` — sell pricing
  Removed the TEMP block; rpgfunk.cs now owns canonical Cap()/round() (round was
  identical anyway).

### 2. keys.cs — 11 malformed keybind lines (`0'` stray tagged-quote)
- [x] `bind break ... 0'` on 11 lines had a stray trailing `'` (opening an
  unterminated tagged string), vs the correct `bind break Down ... MoveBackward 0`
  on line 6. Removed the stray quotes. (Client keybind file; may be unused, but
  it was the only file failing the structural parse check.)

---

## MOVED TO `unused/` (executed by nothing — server, client, or mission)
- Comchat2_.cs — backup variant; Server.cs loads comchat/comchat2/comchat3, not this.
- shopping.cs — duplicate of economy.cs's shop functions (economy is the loaded one);
  also calls fetchData which RM doesn't define.
- local.cs — orphan; execed by nothing. Its only body is `exec(redlist)`.
- REDLIST.CS — only execed by the dead local.cs; sets $adminpassword + remotefetchdata stub.
- Redplanet.cs — the "Red Planet" event (1930 lines); its `exec(redplanet)` in redlist is
  COMMENTED OUT, so it never loads. Dead/disabled feature (also had undefined add_to_fight/
  make_horse). Quarantined, not deleted — restore if you want to revive the event.
- newstuff.cs — 3-line orphan stub (DoInstrumentBoxFunction), execed by nothing.
- C.cs — scratch/notes file (mostly commented-out snippets).

NOTE — deliberately KEPT (not "unused"): client-only files (gui, menu, chatmenu, options,
playersetup, clientdefaults, loadshow, sound, ircclient, observer, commander, keys, sae,
client) are executed by the CLIENT, not the server; and terrain/training/objectives/strings/
editor/registration files are loaded by missions at load-time or by the engine. Those aren't
server-execed but they ARE used, so moving them would break the client or mission loading.
If you want the client-only set relocated too (e.g. this is a dedicated-server-only folder),
say so and I'll move them as a separate batch.

## OPEN / TO INVESTIGATE

### Same-file duplicate function definitions — INVESTIGATED
### 3. Mine.cs — mislabeled bomb onAdd callbacks (REAL BUG, FIXED)
- [x] Pattern in Mine.cs is `MineData BombN` then `function BombN::onAdd` (schedules
  Mine::Detonate). Four callbacks were mislabeled by copy-paste, so those bombs had
  NO onAdd and never detonated:
    - Bomb107 (had `Bomb4::onAdd`), Bomb108 (`Bomb4::onAdd`), Bomb200 (`Bomb4::onAdd`),
      bomb88888 (`bomb612::onAdd`). Renamed each to its own bomb. Now they detonate.
- [keep] rpgfunk.cs `IsInCommaList` 2x, `FixStuffString` 2x — identical bodies (the
  active copy just adds a dbecho). Harmless; optional cleanup, not a bug.
- [x] Chocobo.cs `processMenuViewTradeInfo` 2x — CORRECTION: it IS called (via the menu
  system's `processMenu<HandlerName>` dynamic dispatch — the "ViewTradeInfo" menu built at
  Chocobo L1066). The 1st def (builder, "Trade?" confirm menu) was shadowed by the 2nd
  (the handler), dead-ending the player-to-player TRADE flow. Renamed the 2nd to
  `processMenuTradeChocobo` (the handler name the builder's menu expects). FIXED.

### 4. AccessoryData.cs — `Botboulder35Image::onFire` defined 2x, DIVERGENT (FIXED)
- [x] 232 handled "screech" (screechbolt); 295 handled "dodgethis"/"dodgethisc"
  (dodgethisbolt). Both share shape "boulder"+delay 3.5 => model Botboulder35Image, so
  295 shadowed 232 and SCREECH'S SPELL FIRE WAS DEAD (melee only). Per the comment at
  AccessoryData L214 ("two or more monsters use the same weapon model" => one if/else-if
  fn), MERGED both branches into the single surviving fn and removed the shadowed copy.

### 5. [FLAG] Spells2.cs — `GetBestSpell` defined 2x, DIVERGENT
- [?] 703 (dead): semi-random spell picker that USES the `%semiRandomSpell` param.
  802 (active): weighted "best value" selector that IGNORES `%semiRandomSpell`. The
  active one is more sophisticated (likely the intended newer version), so the
  semi-random code path is effectively gone. Confirm whether that's intended before
  removing the dead 703 (removing it changes nothing at runtime).

- [ ] Client.cs — `Game::endFrame` 5x — client-side file (not loaded by Server.cs);
  low priority, check later.

### Cross-file overrides resolved by load order (verify winner is the intended/correct one)
- [ ] `initSoundPoints` (rpgfunk vs nsound — nsound wins)
- [ ] `gatherBotInfo` (rpgfunk vs Ai — Ai wins)
- [ ] `calcRadiusDamage` (itemevents vs staticshape — staticshape wins)
- [ ] `StaticDoorForceField::onCollision` (Spells2 vs staticshape — staticshape wins)

### Not loaded by Server.cs (dead unless loaded elsewhere — likely harmless)
dm.cs, DeusClient.cs, Comchat2_.cs (in old snapshot), shopping.cs, Client.cs,
newstuff.cs, repack.cs, Carling1/2.cs — their duplicate defs don't conflict
server-side. Confirm none are exec'd from mission/client paths before assuming dead.

### 6. Chocobo.cs — MenuBreed() typo (REAL BUG, FIXED)
- [x] Chocobo.cs:808 (the "Breed" option handler) called `MenuBreed(%Client)`, which is
  defined nowhere — the real function is `MenuBreeding()` (Chocobo.cs:947). So choosing
  "Breed" silently did nothing (breeding menu never opened). Fixed the call name.

### Undefined-function-call scan (bare calls not defined in any script)
Method: strip comments/strings, collect all `function` defs, find bare calls not defined
and not obvious engine builtins. Findings:
- [x] FIXED MenuBreed -> MenuBreeding (above).
- [keep/dead] `fetchData` (shopping.cs) — shopping.cs isn't loaded (economy.cs is), and
  Client.cs:645 `rpgfetchdata(){return False}//RM doesn't support this` confirms RM has no
  fetchData. Dead-code calls. `useItem` (repack.cs) — repack.cs not loaded. Ignore both.
- [?] FLAG `Quest(%Client, %type, false)` — remote.cs:23 (LOADED) calls it, but Quests.cs
  defines NewQuest/FailedQuest/EndQuest, not a 3-arg `Quest`. Real undefined call in a live
  path (remoteQuestChat). Correct target unclear — needs the quest-flow intent.
- [?] FLAG Redplanet.cs — `add_to_fight()` (@1040-41) and `make_horse()` (@460) are called
  but defined nowhere. Redplanet is a special event map; may be an incomplete/unused feature
  or rely on a script that isn't loaded. Verify whether Redplanet is meant to be live.
- [assumed-engine] `RemotePlayAnim` (Ai.cs x4), `updateBuyingList` (Station.cs x3),
  `ClearEvents` (Comchat2.cs), `postAction`, `containerBoxFillSet`, etc. — called in loaded,
  hot code with no near-match; since the mod actually runs, these are almost certainly
  engine/plugin builtins, not bugs. Confirm only if they throw console "unknown command".

### Broader logic sweep — COMPLETE (every load-order file read for undefined vars / wrong logic)

Every file in Server.cs load order was read end-to-end. Fixes below are in addition to #1-6 above.

#### playerdamage.cs (commit 1c75339)
- [x] `Player::onDamage` bot-caster branch: `$ClientData[%Client, SpellDmg] = AddPoints(%sClient,6)`
  used undefined `%Client` (should be `%sClient`); every reader uses `%sClient`. With the
  `== null` early-return this made bot spell attacks via that path do no damage.
- [x] `DegradeableEffects` armor loop scanned `$ClientData[%Client, EquipList]` (the ATTACKER)
  to decide which armor the VICTIM degrades — should be `%dClient`.

#### itemevents.cs (commit 1eba76c)
- [x] `CurePotionStuff` fully non-functional: (1) read `GetWord(%data,...)` from undefined
  `%data` (should be `%svar`); (2) `if(%res!="Petrify" || ...)` always-true De-Morgan bug
  (must be `&&`) so the loop body never ran; (3) `$ClientData[%id, Pertrify]` misspelled
  (should be `Petrify`). Cure potions did nothing.
- [x] `Item::DropItem` equipped-item guard `%item@"0"` should be `%item@$EquipTag`.

#### blackSmith.cs (commit b37f093)
- [x] Multi-tier swing-sound picker computed `%Nsound` but never assigned it back to `%sound`,
  so smithed weapons with e.g. "SoundSwing 5 6" kept the raw multi-word (invalid) sound name.
- [?] `CompleteSmith` L937 `... * multiplier` bareword (=0) instead of `%multiplier`. Making
  the fix would change material consumption for 99-arrow smithing; left as flag (balance).
- [?] `ConvNum` for-increment `%i++ && %len++ && %n=...` short-circuits the first pad on
  single-digit skill (cosmetic display of blacksmith skill %). Left as flag.

#### Player.cs (commit 42aa83b)
- [x] `Player::jump` dismount impulse `Player::applyImpulse(%pl,%mom)` — `%pl` undefined,
  should be `%this`.

#### Mine.cs (commit ad9e702)
- [x] `Bomb3002` had no onAdd — an orphaned `Bomb3001::onAdd` (no such datablock) sat in its
  place, so Bomb3002 never scheduled Detonate (live dud). Renamed to `Bomb3002::onAdd`.

#### AI.cs (commit 9f3d728)
- [x] `AI::HardCodedSkills(%aiId,%class)` declared 2 params but called with 3 and used a
  `%IsMaster` local that was never a param -> master/boss bots never got their stat bonus.
  Added the `%IsMaster` param.
- [x] `RPG::isAiControlled` typo `$BotInfoAiName[%cientId]` (missing l) -> map/path-spawned
  bots (which set $BotInfoAiName but not $SpawnBotInfo) were seen as human players, corrupting
  the PK-legality check in Client::onKilled.
- [?] `AI::SayZone` `if(%cl != %Client ...)` — `%Client` undefined (harmless; sends zone msg
  to the bot itself too). Left.

#### comchat.cs (commit c96df7a)
- [x] `#name` (L363) and `#rename` (L397) used `=` (assignment) instead of `==` in the
  condition (`$OldName[%Client] = ""` / `= true`) -> chocobo naming/renaming silently no-op'd
  AND clobbered $OldName.
- [x] `#steal` read victim stats from undefined `%cl` (`getFinalDEX(%cl)`, `getFinalLVL(%cl)`)
  — victim is `%id`. Steal ignored the target entirely (auto-success). Fixed to `%id`.
- [?] `#steal` `if(String::ICompare($GROUP[%Client],"Rogue"))` looks inverted vs the intended
  Rogue gate (missing `== 0`); balance-sensitive, left as flag.

#### comchat2.cs (commit 314d789)  — all admin-gated (low impact)
- [x] `#fixspellflag` reset `$SpellCastStep[%Client]` (the admin) instead of `%id` (target).
- [?] Flags: `#getexp` guard uses undefined `%id` (read uses `%cl`); `#setpl` checks
  `%id.adminLevel` before `%id` is assigned; `#getpassword`/`#getotherinfo` (level 6, which
  is unreachable — levels cap at 5) read the admin's own data; several set-commands
  `refreshall(%Client)` instead of the target. Left (admin-only).

#### comchat3.cs (commit 1c4ff0a)
- [x] Blacksmith greeting `Client::sendMessage(%Client, "Click Sell...")` missing the
  message-type arg -> instruction mis-sent as type with empty text.
- [x] `getRandomName` retry path recursed without `return` -> returned "" on the collision path.
- [?] Flag: merchant state-2 branch and assassin timeout use REVERSED `$state[%closestId,%Client]`
  key ordering vs the rest (`$state[%Client,%closestId]`) — latent, in near-dead paths.

#### weapons / Spawn / connectivity / gameevents (commit 094209a)
- [x] weapons.cs `ProjectileAttack` fire sound `GameBase::getPosition(%cl)` — `%cl` undefined
  (should be `%Client`); bow/crossbow sound played at world origin.
- [x] Spawn.cs bot-respawn delay `floor(rand*max - min)+min` precedence bug — the min terms
  cancel, giving `[0,max)`; min spawn delay never enforced. Added parens.
- [x] connectivity.cs disconnect "isMimic" revert used `%Client` (undefined; should be
  `%clientId`) -> a player who logs off in a temp mimic race is saved as that race.
- [x] gameevents.cs `RMSetObserver` called as `RMSetObserver(%Client)` (should be `%clientId`)
  at 2 sites, and internally read `getObserverCamera(%clientId)` (should be `%Client`) ->
  invalid/kick-pending players weren't reliably forced into observer mode.

#### Status.cs / Party.cs (commit ebd049f)
- [x] `Status::Mute` added the client to the "Blind" status list (copy-paste) -> Mute never
  ticked down / never expired.
- [x] Four `if(!$ClientData[%Client,X] < -666)` dead conditions (`(!X) < -666` always false)
  -> "You are no longer [Poisoned/Blinded/Muted/Petrified]" messages never showed.
- [x] Party.cs `GetPartyListIAmIn` returned `$ClientData[%Client, partylist]` (caller's OWN
  list) instead of `$ClientData[%id, partylist]` (the party they're a member of) -> #party
  chat reached nobody for non-owner members.
- [?] Systemic (NOT changed, balance): status `X += Cap(X+time,0,max)` (4 status fns + cage +
  alcohol) over-accumulates duration vs an intended `X = Cap(...)`.

#### Chocobo.cs (commit b2871ca) — alpha subsystem
- [x] Trade-flow duplicate handler (see #67 correction above).
- [x] `Chocobo::Clear`: `$ChocoboTakeCare[%Client, %i]` used undefined `%i` (should be
  `%Choco`); `%name` undefined at the $funk::var save writes (added
  `%name = Client::getName(%Client)`).
- [x] `Chocobo::Trade` and `Chocobo::Breed`: `%Hostname = Client::getname(%ClientBuyer)`
  (should be `%ClientHost`) -> wrong name shown in trade/sell/breed messages.
- [?] Flags: `if(%char == -1)` loop-stop in trade/breed player menus never fires (>10 players
  edge case); `MenuBreeding` references undefined `%opt`; the both-inventories-full trade
  edge case in `Chocobo::Switch` self-copies slot 20; `Armor::dismount` riddled with
  undefined `%object`/`%cl` (likely dead — Armor::jump trigger is commented out). Left.

#### AccessoryData.cs (commit 19f668e)
- [x] Botboulder35Image::onFire duplicate (see #4 above).
- [?] Flag: `greensword040Image::onFire` handles Drag_Sword (shape=longstaff) and
  `longstaff025Image::onFire` handles Punisher (shape=greensword) — the fn names don't match
  those weapons' shape+delay-derived model names, so their spell projectiles may not fire.
  Depends on exact MakeItem model-name derivation; left as flag (weapons still melee).

#### carling2.cs — PK-logging (Carling, 2004) — BROKEN, left as flag
- [?] `pk::monitor` counter `while($PK[%kname,%i++]...)` starts at -1 so always ends at 0, and
  the export key format `$PK["[k,i]"]` != the read format `$PK[k,i]`. Net: `(%i%5)==0` is
  always true, so a PKer is caged 900s on EVERY qualifying low-level kill (not every 5th).
  `pk::bancheck` uses undefined `%kname`. A correct fix needs a rewrite of the key handling
  (untestable here) and changes moderation behavior — flagged, not blindly rewritten.

#### DeusKeys.cs — DEAD code, left as flag
- [?] `Keys()` (the `%i+3` no-op increment + `GetWord(%i++)` mis-parse of key triples) is
  never called anywhere (only a commented example); `$KeyList` is only save/load-persisted
  (an always-empty list). Broken but unreachable — not worth fixing an abandoned feature.

#### Reviewed clean (no changes): Accessory.cs, economy.cs, remote.cs, weaponHandling.cs,
Vehicle.cs, Turret.cs, Beacon.cs, StaticShape.cs, Station.cs, Moveable.cs, Sensor.cs,
InteriorLight.cs, townbots.cs, Intro.cs, Mission.cs, plugs.cs, version.cs, weight.cs,
mana.cs, hp.cs, Stamina.cs, rpgstats.cs, Quests.cs, playerspawn.cs, carling1.cs (data-typo
flag only: Pear/Platnium/Emeral in mob material drop tables), ChocoboArmor.cs (pure data).

## POST-REVIEW FIXES (Jul 4, 2026 — Fable review pass over the sweep)
### Chocobo jump-dismount hardened (Chocobo.cs)
- [x] Armor::jump now handles BOTH dispatch cases: %this = ridden chocobo AND %this =
  the mounted RIDER (players share className "Armor"; the rider resolves its chocobo
  via Player::getMountObject). Either way the full Armor::dismount restore runs
  (control object, weapon, driver flags) instead of stock Player::jump's raw unmount.
- [x] Armor::dismount only Chocobo::Delete's the bird when the rider OWNS it
  ($ChocoboSpawn[%cl] == %this). Riding someone else's chocobo no longer deletes the
  rider's own saved bird; the ridden one is released ($isChocobo=true, clLastMount
  cleared) so it can be mounted again. STILL NEEDS LIVE TEST (whether the engine
  fires the jump callback at all in the ride configuration is unverified).
### CompleteSmith batch fix (blackSmith.cs)
- [x] The arrow/quarrel batch check compared getword(%tempsmith,1) — a COUNT in the
  "item count item count" list — so it never matched, and %multiplier was clobbered
  to 1: players paid cost×N for #smith N but always received 1 item, and ammo never
  batched 99. Now: detection via findSubStr over the whole list; the paid %amt is
  honored (give N items, or 99×N for ammo; materials consume count×N capped 1..99).
  NOTE: inventory caps may still limit ammo batches past 99 — retune/verify on test.
