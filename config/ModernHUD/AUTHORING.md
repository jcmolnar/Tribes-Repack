# ModernHUD — the complete pack-authoring reference

**What this is:** everything you need to build a custom HUD for this client, and
an account of how the framework works underneath so you can tell what is cheap,
what is expensive, and what is impossible.

Every command listed here was read out of the registration tables in the source,
not from memory. Where a name looks like it should exist and does not, that is
said explicitly — an API list that quietly includes wishful entries is worse than
a short one.

**Companion docs:** `re/modernhud_pack_format_v1.md` (the frozen manifest spec),
`re/modern_hud_framework_plan.md` (why the framework exists), and
`re/vector_hud_buildout.md` (a complete worked example, including its mistakes).

---

## 1. What a pack IS

```
config/ModernHUD/Packs/<id>/
  pack.json       manifest -- identity, parts, anchors, asset map
  hud.cs          the script the client executes
  components.cs   (optional, recommended) the parts OTHER packs may borrow -- §11
```

A pack may reference shared art under `config/ModernHUD/Assets/` and carried data
layers under `config/ModernHUD/Core/Data/`, but it owns only its own folder —
which is what makes uninstalling one safe.

**Read §11 before you ship.** Options → **CONFIGS/HUDS** (the HUD designer) draws
your pack inside the Options page, at other screen sizes, with demo data, and lets
players move, resize, hide, re-anchor and *swap* your parts for other packs'. A pack
that ignores §11 still works in game but shows up broken there.

`"authoring": "manual"` in the manifest means you wrote `hud.cs` by hand and the
generator must never overwrite it. `"generated"` means it was produced from the
manifest by `tools/modernhud_pack.py`.

**Immediate mode.** A pack does not create SimGui controls. It draws, every
frame, into the play GUI. There is no retained widget tree, no authored extents,
no persistent clip boxes. The consequences are worth internalising:

- **A guard that skips work also skips DRAWING.** A retained control keeps its
  last value; an immediate one does not exist unless it is drawn this frame. Any
  `if (...) return;` early-out you inherit from legacy config code is a bug here.
- **Per-frame cost is real.** The draw runs once per frame per part. It is script,
  so keep it to arithmetic and draw calls; do not walk object sets in it.
- **State lives in globals**, because there is no widget to hang it on. Prefix
  everything with your pack id.

**Deploy:** author in the repo at `config/ModernHUD/Packs/<id>/`, then run
`./deploy-modernhud.ps1`, which copies the whole master tree (framework, packs,
registry, shared data) into every game tree. Never hand-copy a pack: a pack
without its matching `Framework.cs` is the exact failure the script exists to
prevent.

---

## 2. The lifecycle — what the client calls, and when

Your `hud.cs` must define these. The framework calls them unconditionally, so a
missing one is a per-frame console error.

| function | when | purpose |
|---|---|---|
| `ModernHUDPack::draw(%screen)` | every frame | draw everything. `%screen` is `"w h"` in surface pixels |
| `ModernHUDPack::ownsSlot(%value)` | per part, per frame | return true if this pack still owns that HUD slot |
| `ModernHUDPack::init()` | on load | apply client-wide settings |
| `ModernHUDPack::prefs()` | on load | the pack's own `$pref::miniMap*` / `$pref::hudPositions*` |
| `ModernHUDPack::stockHuds()` | on load + `eventGuiOpen_PlayGui` | show/hide the stock huds |
| `ModernHUDPack::detachRetained()` | on load | remove legacy containers this pack replaces (may be empty) |

Boot order at the bottom of `hud.cs`:

```
ModernHUD::attach("eventGuiOpen_PlayGui", "ModernHUDPack::onPlayGuiOpen");
ModernHUDPack::prefs();
ModernHUDPack::stockHuds();
ModernHUDPack::init();
```

★Bind `eventGuiOpen_PlayGui`, never `eventGuiOpen` plus a gui-name test.★
**Two** independent firers raise `eventGuiOpen`, with **different spellings**:

| firer | argument |
|---|---|
| the engine, using the control's real name (`simGuiCanvas.cpp:907`) | `playGui` |
| Presto, using a hardcoded bare word (`events.cs:681` via `OpenAGui(PlayGui)`) | `PlayGui` |

A string **value** comparison is case-**sensitive** even though **name** lookup
is not (`compare()` falls through to a plain `strcmp` for two non-numeric
strings, `engine/console/code/eval.cpp`). So the old `%gui == "playGui"` test
matched the engine's spelling and silently ignored Presto's. It *worked* —
measured live, `oldSeen=playGui`, one hit per transition — but only because the
two spellings happen to differ, and a pack that had spelled it `PlayGui` would
have matched the other firer instead. Presto fires an argument-free
`eventGuiOpen_PlayGui` from the same function (`events.cs:746`): binding that
removes the string test altogether, at the same frequency.

Use `ModernHUD::attach` rather than a raw `Event::Attach`: the framework revokes
tracked handlers in `detachAll()` on unload, so a hook cannot outlive its pack.

★`stockHuds()` must declare the WHOLE set, not just the huds you want on.★ Stock
visibility is global client state; a pack that lists only its own leaves the rest
wherever the previous pack put them. That is a measured defect, not a
hypothetical.

**Unload** is handled for you by `ModernHUD::unload()`: it removes retained edit
handles, revokes event attachments, clears the settings registry, purges orphaned
handles by type, and sweeps dead `$Hud` entries. You do not call it.

---

## 3. Drawing — the ScriptGL command set

These are the complete set of `gl*` console commands, from the registration table
in `engine/SimGui/code/scriptGL.cpp`. They are only valid during a draw.

### Colour and shapes

| command | notes |
|---|---|
| `glColor4ub(r, g, b, a)` | sets the colour for the next primitive. 0-255. Alias: `glColor` |
| `glRectangle(x, y, w, h)` | filled axis-aligned rect |
| `glAngledPolygon(x1,y1, x2,y2, x3,y3, x4,y4)` | arbitrary 4-point convex quad — chevrons, chamfers, parallelograms |
| `glGradientRect(x, y, w, h, r2,g2,b2,a2 [, "h"])` | two-stop linear gradient. Stop one is the CURRENT `glColor4ub`; default vertical, `"h"` for horizontal |
| `glShape(x, y, w, h [, role [, state]])` | draw a widget in the active SDF theme's shape language. role 0-5, state 0-3 |

`glAngledPolygon` and `glGradientRect` were added for pack authors: as
axis-aligned rects the same shapes are a stair-step of dozens of draws, and a
gradient otherwise needs a PNG (which reintroduces a missing-asset failure mode).

### Text

| command | notes |
|---|---|
| `glSetFont(family, pixelHeight)` | select/build a **TrueType** atlas at that exact size |
| `glDrawString(x, y, str)` | draw with the current font. Parses inline `<rrggbb>` / `<rrggbbaa>` colour tags |
| `glGetStringDimensions(str)` | `"w h"` in the CURRENT font — call `glSetFont` first |
| `glFontExists(family)` | `"1"` if that TrueType family is installed |
| `glDrawMarkup(x, y, width, markup, alpha)` | the engine's own `.pft` markup renderer |
| `glGetMarkupDimensions(markup)` | `"w h"` for a markup string |

★Use `glSetFont` + `glDrawString` for anything that scales.★ A `.pft` is a
fixed-size bitmap font, so scaling a part *magnifies* its glyphs and they go
blocky. `glSetFont` rasterizes a fresh GDI atlas at the requested size, so
scaling re-renders instead. This is the single biggest visual-quality decision in
a pack.

★A colour tag must be exactly 8 or 10 characters.★ If you multiply alpha by an
opacity factor you will get fractional values; floor before converting to hex, or
the tag becomes malformed, gets stripped as unknown, and your text renders in
whatever colour was last set.

**Font cache:** atlases are cached per `(family, px, weight)`, 32 entries, LRU. If
your sizes are derived from a slider, quantise them (`floor(px/2)*2`) so a
continuous control does not mint an atlas per step.

### Images

| command | notes |
|---|---|
| `glDrawImage(x, y, w, h, path [, alpha [, tint\|"keyblack"]])` | w/h <= 0 uses native size. Alpha accepts 0..1 or 0..255 |
| `glDrawImagePart(...)` | source-cropped draw, for bar art |
| `glGetImageDimensions(path)` | `"w h"` |

### Input

| command | returns | notes |
|---|---|---|
| `glMousePos()` | `"x y lmb rmb"` | cursor in **surface pixels**, already converted from the content control's space. `""` if no canvas |
| `glMouseRMB()` | `"1"` / `""` | right button state |
| `glPollWheel()` | notches since last poll | `""` if none |
| `glPollHotkey()` | the fired DIK | for an armed key |
| `glTextInput(1\|0)` | — | capture keys into the poll queue |
| `glTextPoll()` | `"c<char>"` / `"k<dik>"` / `""` | drain one event |
| `glSetTalkKey(dik)` / `glSetTalkKey2(dik)` | — | arm a raw DIK to open the chat composer |

`glMousePos` is what makes a **clickable** script UI possible — see §8.

### Timing and transform

| command | notes |
|---|---|
| `glTicks()` | milliseconds, wall clock. Use for animation; it is frame-rate independent |
| `glPartScale(originX, originY, sx [, sy])` | scale subsequent draws about a point. Scale 1 = identity reset, and it also drops the part style (`glPartStyle` hide/opacity) |

★`glPartScale` persists to the END of the whole ScriptGL pass.★ `ModernHUD::part`
pushes one per part, so anything you draw after your last part call inherits the
last part's scale. If you draw freehand at the end of `draw()`, call
`glPartScale(0, 0, 1)` first. This has bitten a real pack: a 1.14x part scale
moved a centred panel off-screen and desynchronised every mouse hit-test.

`glEnable` / `glDisable` / `glBlendFunc` exist as **no-ops**, so legacy scripts
that call them do not error.

---

## 4. Framework API (`ModernHUD::*`)

### Placement

```
ModernHUD::part(name, anchor, offsetX, offsetY, w, h, screen)   -> "x y"
```
The one call a normal part makes. Resolves the anchor, creates/updates the
retained edit handle (so the player can drag it and the K-editor can select it),
applies the saved user scale, and returns where to draw.

```
ModernHUD::place(anchor, offsetX, offsetY, w, h, screen)        -> "x y"
```
The anchor maths **without** a handle. Use for something that must not be movable
— a reticle-centred cluster is the case that proved it: a draggable reticle gets
dragged off the aim point, and then "reset positions" cannot recover it either.

```
ModernHUD::dockTo(name, target, dx, dy, fallback, partW, partH) -> "x y"
ModernHUD::hide(name)
```
`dockTo` pins a part to another control's live position and extent (a frame drawn
around the chat box). `hide` is null-safe — hiding a handle that was never
created does nothing.

**Anchors** (exactly these nine): `top-left` `top-center` `top-right`
`center-left` `center` `center-right` `bottom-left` `bottom-center`
`bottom-right`. Note it is `center`, **not** `center-center`; an unknown anchor
falls through to the raw offset, parking the part at the top-left corner.

### Drawing helpers

| function | notes |
|---|---|
| `ModernHUD::markup(x, y, width, value, alpha)` | `.pft` markup with justification |
| `ModernHUD::imageRect(x, y, w, h, path, alpha, tint)` | fixed-size image |
| `ModernHUD::imageAt(x, y, path, alpha, style)` | native-size image |
| `ModernHUD::bar(x, y, w, h, path, alpha)` | source-cropped bar |
| `ModernHUD::digitsBox(x, y, folder, value, alpha, spacing, box, align)` | per-digit number art |
| `ModernHUD::digitsAt / digitsWidth` | lower-level digit helpers |

### Data layers and events

```
ModernHUD::require("ModernHUD/Core/Data/Team.cs")
ModernHUD::attach(event, function)
```
`require` execs a shared data layer once. `attach` registers an event handler
**through the framework**, so `unload` can revoke it — a raw `Event::Attach` from
a pack leaks a handler into the next pack, which is the exact legacy defect this
framework exists to remove.

### Settings — the part most authors will want

```
ModernHUD::setting(type, prefKey, label, default, spec, apply [, part])
```

| arg | meaning |
|---|---|
| `type` | `"enum"` \| `"bool"` \| `"int"` |
| `prefKey` | a `$pref::` variable **without** the `$` — `"pref::Vector::Theme"` |
| `label` | shown in Options |
| `default` | seeded only if the pref has never been set |
| `spec` | enum: `"Label\|value;Label\|value;..."` · int: `"min\|max\|step"` · bool: unused |
| `apply` | console command run whenever the value CHANGES (may be `""`) |
| `part` | *optional* — the part this setting belongs to (§11.4). Omit for whole-HUD settings |

Declaring a setting gives you a row on **Options → Configs → `<Pack>` Settings**,
**and** a row in the in-game **K menu** (§8) — one declaration, both surfaces, and
no menu code to write. The value is captured by HUD presets. Prefs persist for free — the client's
exit-time `export("pref::*")` sweep saves every `$pref::`, so a pack never needs
`export()` (which the format forbids anyway).

★Three rules that are not obvious:★

1. **A pref exposed as a setting must NOT also be listed in the manifest's
   `prefs` block.** `prefs` re-runs on every pack load and would force the value
   back at each boot, so the player's choice would appear not to stick.
2. **Defaults are seeded, not defaulted-at-read.** This is deliberate: see the
   `== 0` trap in §7.
3. **A pack's first launch freezes its defaults into `ClientPrefs.cs` forever.**
   Ship a better default later and nobody who already ran the pack will see it.
   Provide a `YourPack::defaults()` that force-writes current values, or you will
   spend a session wondering why a changed default does nothing.

---

## 5. Game state — what you can actually read

### Live player values

Exported every frame by `CfgSyncHudVars_now` (`program/code/kronosNativeCmds.cpp`).
★Their exact semantics matter and are not what you would assume:★

| variable | type | meaning |
|---|---|---|
| `$health` `$energy` | int 0..100 | 0 when not in game |
| `$speed` | int | **world units/sec** (`getLinearVelocity().len()`), NOT km/h. Walking ~25, a held ski line 100-200 |
| `$Weapon::Ammo` | int | **`-1`** nothing mounted, **`0`** mounted but no ammo type (energy weapons). **Never `""`** |
| `$damageFlash` | float 0..0.76 | live "being hit right now", straight off the wire. 0 when clean |

`$damageFlash` is the correct way to react to damage. Watching `$health` drop
misses chip damage absorbed by armour, misfires on healing, and is a frame late
by construction. (Do not confuse it with `$pref::damageFlash`, which is a user
intensity knob, not state.)

### Inventory and weapon

| command | returns |
|---|---|
| `getItemCount(name)` | count of an item by display name — `"Grenade"`, `"Beacon"`, `"Mine"`, `"Repair Kit"` |
| `getMountedItem(slot)` | item id in an image slot; slot 0 is the live weapon |
| `getItemDesc(id)` | display name of an item id |
| `getItemType(id)` | item type |

The console is **case-insensitive**, so `GetItemCount` and `getItemCount` are the
same command.

### Client and team

`Client::getName` `Client::getTeam` `Client::getGender` `Client::getSkinBase`
`Client::getControlObject` `Client::getFirst` `Client::getNext`
`Client::getGuiMode` `Client::centerPrint` `Client::sendMessage`
`getManagerId` `Group::objectCount` `Group::getObject` `Group::iterateRecursive`

### Carried data layers

`ModernHUD::require("ModernHUD/Core/Data/Team.cs")`:

`Team::Friendly()` `Team::Enemy()` `Team::Score(team)` `Team::Size(team)`
`Team::Flag::Location(team)` `Team::Flag::Timer(team)`
`$Team::Name[team]` (0-indexed at read time)

`Team::Flag::Location` returns `"home"`, `"field"`, or a client id (the carrier).

`ModernHUD::require("ModernHUD/Core/Data/Timer.cs")`:
`Timer::New` `Timer::Inc` `Timer::Dec` `Timer::FormatSeconds`

### Events

`ModernHUD::attach` accepts: `eventGuiOpen` `eventGuiClose` `eventPlayMode`
`eventCommandMode` `eventObjectivesMode` `eventInventoryMode` `eventItemReceived`
`eventItemDropped`.

### Client-wide settings a pack may drive

Save the old value before writing any of these, and restore it in your
`restore()` — a pack that permanently rewrites client-wide globals is a pack you
cannot uninstall.

| namespace | what |
|---|---|
| `$pref::Hud::Color*` | engine-wide HUD colour theme (Primary/Dim/Accent/Warn/Text/Pass) |
| `$mj::shownames`, `showhpbars`, `showjetbars`, `bar_width`, `bar_height`, `bar_border_width`, `fontdefault`, `passhelper` | nameplates and the world layer |
| `$mj::DrawWeapon`, `$mj::WeaponAlpha` | first-person weapon visibility. **`WeaponAlpha` is a float 0..1, not a byte**, and `$mj::` is NOT persisted by the pref sweep |
| `$pref::miniMapWidth`, `Zoom`, `Rotate`, `Square`, `Compass`, `miniMapAlpha` | the minimap control |
| `$pref::hideCrosshairArt` | suppress the stock reticle bitmap |
| `$xChat::*`, `$pref::ChatDisplay*` | chat placement and fade |

★Your `$mj::` values are the "Pack" default, not the last word.★ Options has a
Pack / On / Off row for names, health bars, jet bars, health % text, crouch-only bars,
both pass helpers, HUD brackets and the first-person weapon (`$pref::mj::<knob>`,
`$pref::hudBrackets`). Pack (the default) shows whatever your pack wrote; On/Off win
over it on every pack. Keep writing `$mj::` exactly as before.

★`$mj::eyesight` / `eyesightbars` are NOT pack knobs.★ They drop the sensor gate on
nameplates, which is a base-game advantage, so the engine honours them only under our own
cockpit packs (`kEyesightPacks` in fearGuiCrosshair.cpp) while the server is streaming mech
state. A pack that sets them anywhere else gets stock plates. A generated pack's `mj` block
refuses them outright (tools/modernhud_pack.py `MJ_PACK_KNOBS`).

★Two minimap traps.★ `$pref::miniMapVisible` is the **legacy canvas overlay**, a
second undraggable minimap — not "show the minimap". The real control is shown
through `stockHuds`. And `$pref::miniMapAlpha` is a 0..1 float; a value above 1 is
read as a 0..255 byte, so writing a percent gives you 80/255.

★`crosshairHud` is not just the crosshair.★ `FearGui::Crosshair::onRender` also
draws the entire nameplate system — names, health/jet bars, pass helper,
friend/foe skulls, target acquisition. Hiding the control to remove the reticle
takes all of that with it. Use `$pref::hideCrosshairArt`.

---

## 6. Useful console utilities

`String::toUpper` `String::toLower` `String::Length` `String::getSubStr`
`String::findSubStr` `String::replace` `String::Trim` `String::lpad`
`String::escapeFormatting` `String::Explode` `String::getWord`
`String::getWordCount` · `getWord` `floor` `round` `min` `max` `sqrt` `pow`
`getVariable(name)` `setVariable(name, value)` · `getRealTime` `getRealMillis`

★Three of these are SHADOWED by Presto at runtime and are NOT the native
implementations.★ A script `function X` replaces a native command of the same
name outright (`Dictionary::addFunction` placement-news the entry and wipes the
native callback), and Presto loads in every session, so what a pack actually
calls is:

| name | who really answers | why it matters |
|---|---|---|
| `String::toLower` | `Presto/LevelUpSave.cs` | a per-character script loop over `String::getSubStr` — same ASCII result, far slower; do not call it per frame |
| `String::getWordCount` | `Presto/upgrade/extra.cs` | counts by looping until `getWord(...) == -1`, so a word that *is* `-1` ends the count early — not a native word count |
| `String::removeBetween` | `Presto/ATKText.cs` | full script reimplementation |

Prefer `String::Explode` (native, and it reports the field count) over
`String::getWordCount` when the input is untrusted. The other names in the list
resolve to the native commands.

`getVariable` / `setVariable` are the console's only dynamic variable access —
there is no dynamic *assignment* in the grammar (`*expr(args)` is a dynamic
CALL), which is why packs otherwise have to write save/restore lists out longhand.

★Run any wire-sourced string through `String::escapeFormatting` before putting it
in markup.★ Player and team names come off the network and can contain markup.

---

## 7. The traps — read this section before writing code

Each of these cost a real debugging session.

**`$pref::X == 0` matches an UNSET pref.** `compare()` promotes the whole
comparison to float as soon as either side is a numeric literal, and
`evalFloat("")` is 0. Test `!= ""` **first**, as a string:
```
%v = $pref::Foo;
if(%v != "" && %v == 0) { /* explicitly disabled */ }
```

**An unset counter is `""`, and `$A[""]` is a DIFFERENT variable.** `$A[%i]` builds
its name by concatenating the base with the evaluated index, so with `%i` empty
the name is plain `$A`, not `$A0`. Seed counters before an indexed write.

**`default` is a reserved word.** Also `case`, `switch`, `before`, `after`,
`halt`. Using one as an array subscript is a parse error that aborts the rest of
the file — taking every function defined below it with it.

**A braceless `if` at top level is a silent syntax error.** Inside a function it
is fine.

**Ternary `?:` and `for` work** in this native client (1.40-parity additions). They
do **not** exist in the browser client, so a pack meant for both cannot use them.

**`return true` works under `&&` and `if`** — the console special-cases the string
`"True"` as 1.

**Fractional alpha corrupts colour tags.** Floor before hex conversion.

**Don't hide a control to remove its art.** See `crosshairHud` above.

**Reset must undo scale as well as position** if you offer resizing —
`$pref::hudScale<name>` is written on drag-release and nothing else clears it.

---

## 8. The K menu — you get it for free

**Do not write a settings panel.** The framework owns one, it is drawn for every
pack, and its rows ARE your `ModernHUD::setting` registry (section 4). Declare
settings and a player can change them in game, on the key they already press.

This was Vector's hand-written panel — 250 lines of drag, hit-test and stepper
code that only one pack had, driving rows that duplicated the registry by hand.
It now lives in `Framework.cs` (`ModernHUD::menu`) and every pack shares it.

**What you get without writing anything:**

- a draggable, clamped panel on `K`, drawn over the HUD it configures
- one row per registered setting, `[-][ value ][+]`, wrapping enums / clamping ints
- **HUD opacity** and **HUD size** rows, supplied by the framework, that scale
  every part drawn through `ModernHUD::part` / `imageRect` / `bar` / `markup` /
  `digitsBox`
- RESET DEFAULTS, restoring every row to the default your pack declared
- paging when the rows outnumber the screen

**What you may override, and nothing else:**

```
// Palette — set these from your theme; unset means the framework's blue.
$ModernHUD::MenuPrimary = "0 200 255";
$ModernHUD::MenuDim     = "0 62 78";
$ModernHUD::MenuAccent  = "255 190 60";
$ModernHUD::MenuText    = "235 245 255";
$ModernHUD::MenuWarn    = "255 60 60";
$ModernHUD::MenuTitle   = "VECTOR";      // heading; defaults to the pack id
$ModernHUD::MenuFont    = "Verdana";     // any face glFontExists() confirms

// Decline a framework row you already provide. Two controls scaling the same
// pixels is worse than none.
$ModernHUD::OwnOpacity = 1;
$ModernHUD::OwnScale   = 1;

// Your own panel chrome, if the default frame is not your look.
function ModernHUDPack::menuFrame(%x, %y, %w, %h, %head) { ... }

// Extra work RESET DEFAULTS cannot know about (engine prefs you drive, a
// derived palette). Called after the rows are restored.
function ModernHUDPack::menuReset() { ... }
```

`$Config::HudListOwned` is set for you as soon as you register your first row,
and cleared on unload. A pack that registers no settings still gets the two
framework rows; the stock hud list is never shown for a master pack.

**Applies are not yours to run.** The native watcher
(`modernHudPacks.cpp MHSettings_tick`) notices any registered pref changing and
runs that row's apply command — the same path the Options page uses. Running it
from a menu handler as well double-fires every one.

## 9. How it works underneath

**The render hook.** `ScriptGL_renderHook` runs once per play-GUI frame. It sets
the surface, manager and canvas statics, then dispatches
`ModernHUD::onDraw(%screen)`, which calls your `ModernHUDPack::draw`. Every `gl*`
command is only valid inside that window.

**Handles.** `ModernHUD::part` creates a `FearGui::ModernHudHandle` — a retained
`HudCtrl` that exists solely as a drag target and K-editor hit box. It carries no
art. Identity is the numeric SimObject id, never the name, because names
containing `::` do not round-trip through `isObject`.

**Positions** live in `$pref::hudPositions<name>` and scales in
`$pref::hudScale<name>`, both persisted by the pref sweep. `ModernHUD::part`
re-reads the scale every frame; a retained `HudCtrl` caches it, which is why
resetting one requires touching the live control as well as the pref.

**Slots.** `$pref::HudSlot::<slot>` names which pack owns each canonical slot
(`healthenergy`, `weapon`, `ctf`, `items`, ...). Your `ownsSlot` decides whether
to draw. A part may claim several slots — then it must yield if **any** of them is
borrowed, or the borrowed control renders underneath yours.

**Theme integration.** `glShape` draws in the active SDF theme's shape language,
so a pack can match whatever skin the player selected rather than hard-coding a
look.

---

## 10. Checklist for a new pack

- [ ] `pack.json` validates: `python tools/modernhud_pack.py --validate <path>`
- [ ] All six `ModernHUDPack::*` lifecycle functions defined
- [ ] `stockHuds()` declares the **whole** stock set
- [ ] Every part yields its slot with `else ModernHUD::hide(<handle>)`
- [ ] Client-wide writes are saved before writing and restored in `restore()`
- [ ] Text that scales uses `glSetFont` + `glDrawString`, not `.pft` markup
- [ ] Wire strings pass through `String::escapeFormatting`
- [ ] No pref appears in both the `prefs` block and a `ModernHUD::setting`
- [ ] A `defaults()` exists so shipped defaults can be re-applied later
- [ ] Tested at two resolutions and after a pack swap away and back
- [ ] **HUD designer (§11):** every part goes through `ModernHUD::part` (or records itself
      with `ModernHUD::pvRecord`), and is listed in `pack.json` `parts[]` with its `handle`
      and `slot`
- [ ] Nothing in `draw()` writes a pref, creates/moves a handle, or schedules anything
      while `$ModernHUD::Preview` is set
- [ ] Game state is read through the getters the demo answers (§11.2), or the pack has a
      demo branch on `$ModernHUD::PvDemo`, or declares `$ModernHUD::PreviewNote`
- [ ] Borrowable parts live in `components.cs` under your own namespace, with a
      `ModernHUD::component` line each (§11.3) — `python tools/modernhud_components.py`
      splits an existing pack and verifies the result
- [ ] Part-specific settings carry their `part` tag (§11.4)
- [ ] Checked on Options → CONFIGS/HUDS: main menu, all four Demo scenarios, Screen at
      1280x720 and 4K, and each of your components borrowed into another pack

---

## 11. The HUD designer — making a pack work in Options → CONFIGS/HUDS

The designer (`program/code/fearGuiModernOptions.cpp`, scope doc
`HUD-DESIGNER-SCOPE-2026-09-23.md`) draws the loaded pack **inside the Options page**
through `ScriptGL_renderStage`. Your `ModernHUDPack::draw(%screen)` runs exactly as in
game, with four differences that each broke a shipped pack before this section existed.

### 11.1 The preview pass — rules for `draw()`

`$ModernHUD::Preview` is `1` for the duration of the pass.

- **`%screen` is the PREVIEW's screen**, not the window's: a player can preview 1280x720
  or 3840x2160 on a 1080p monitor. Lay out only from `%screen` (`ModernHUD::place` /
  `ModernHUD::part` do). Never read the window size another way.
- **Place parts with `ModernHUD::part`.** In a preview it is side-effect free: it never
  creates, moves, resets or fit-checks a retained handle (`ModernHUD::previewPos` answers
  instead — the live spot, else the saved pref, else your authored spot, clamped onto the
  screen the way the handle clamps in game). It also records the part for the designer's
  hit boxes (`ModernHUD::pvRecord`).
  ★**A pack with its own placement helper must do all three itself.**★ Overstep keeps its
  own `handle()`; until it deferred to `ModernHUD::handle` in preview, every Options frame
  created and moved LIVE handles from preview coordinates, and none of its parts could be
  selected. And until it called `ModernHUD::partStyle`, the designer's Size / Opacity / Hide
  rows did nothing to its parts, in the preview AND in game (measured: RepKit stayed fully
  opaque at opacity 22). The pattern (Overstep's `components.cs`):
  ```
  if($ModernHUD::Preview)
  {
     %at = ModernHUD::handle(%name, %defaultPos, %w, %h);   // side-effect free here
     %s  = ModernHUD::partStyle(%name, %at);                  // size, hide, opacity
     ModernHUD::pvRecord(%name, %at, %w * %s, %h * %s, "");   // selectable in the designer
     return %at;
  }
  ...live placement...
  ModernHUD::partStyle(%name, %published);                    // the same in game
  return %published;
  ```
  A caller that fetches several handles before drawing (Overstep's three status plates)
  must re-issue `ModernHUD::partStyle(name, at)` before each part's own draws — otherwise
  all of them draw in the style of the LAST handle fetched.
  ★Retrofitting `partStyle` into a pack that never had it makes old saved sizes take
  effect.★ Sizes a player saved while they did nothing visible are usually junk (someone
  scrolled a part down to the 0.25 floor waiting for it to change). Clear them once, and set
  the legacy claim marker so the unqualified value is not re-claimed — see
  `Overstep::forgetInvisibleSizes` in `Packs/overstep/hud.cs`.
- **No side effects.** Do not write prefs, swap client-wide settings, `schedule()`, or
  enter/leave a mode from `draw()` while previewing. Starsiege Cockpit's `SSC::enter()`
  rewrites the crosshair/IFF/nameplate prefs; calling it from a preview pass changed the
  player's live game from the Options page. Guard mode switches with
  `if(!$ModernHUD::Preview)`.
- **The K panel, toasts, music panel and net stats are NOT drawn** in a preview, and
  `glMousePos()` returns `""` — the Options cursor is not aimed at your HUD.
- **`.pft` markup, `glDrawImage` and TrueType all work** — the pass re-syncs the GFX
  texture cache the Options page's own drawing leaves stale (`GFX_forgetGLState`). If a
  new draw path renders as solid blocks or white boxes ONLY in the designer, that cache is
  the first suspect.

### 11.2 Demo data — what the preview can show out of game

At the main menu there is no player, inventory or team. While a **demo** preview draws
(`$ModernHUD::PvDemo`, set by `ModernHUD::previewBegin`), the framework answers:

| read | demo answer |
|---|---|
| `$health` `$energy` `$Weapon::Ammo` `$speed` `$damageFlash` | per scenario (Full health / Low health / Flag carried / No ammo) |
| `$compassHeading` `$compassSin` `$compassCos` `$sensorPing` | a heading, a ping in Low health |
| `getItemCount(desc)` | a medium CTF loadout (disc/chain/GL/blaster, ammo, grenades, mines, beacons, kit, energy pack) |
| `getMountedItem(0)` → `getItemDesc(...)`, `getItemType(desc)` | the mounted weapon (fixture ids ≥ 30000 round-trip) |
| `getManagerId()`, `Client::getName(id)`, `Client::getTeam(id)` | you (team 0), "Enemy Heavy", "Teammate" |
| `Team::Score`, `Team::Flag::Location/Timer`, `$Team::Name` | scores, flag home/field/carried per scenario |

So **read game state through those getters** and the preview fills in for free (the
native side is `ScriptGL_stageDemo` + the fixture block in `FearPlugin.cpp`).
State that arrives any other way — a server push (`$MMC::*`), a `remoteEval` cache, an
object walk — is empty in demo. Then either:

- add a demo branch: `if($ModernHUD::PvDemo) { ...placeholder values... }` (Starsiege
  Cockpit's `SSC::demo()` also honours its own `$pref::ssHudDemo`), or
- declare a note the stage shows instead of an unexplained empty backdrop:
  ```
  $ModernHUD::PreviewNote = "A caster HUD: it draws the telestrator while you observe a match.";
  ```
  (cleared on unload). A pack that makes **no draw call** at all gets a general note
  automatically; one that draws nothing on purpose (Observer) should say why.

Caches keyed on `glTicks()` (Ascend's 300 ms weapon scan) are shared by the preview and
the live HUD — keep them short, or key them on `$ModernHUD::PvDemo` too.

### 11.3 Parts, slots and mix-and-match (`components.cs`)

Players can hand any **slot** (`healthenergy`, `weapon`, `ctf`, `items`, `clock`,
`minimap`, `chat`, … — `s_slots[]` in `configModules.cpp`) to another pack's part: the
selected part's **"Drawn by"** row writes `$pref::HudSlot::<slot> = "<pack>/<component>"`.

**Your manifest.** Every part in `pack.json` `parts[]` needs its `handle` and `slot`
(a string or a list — the first is the one it is borrowed for). That map is how the
designer knows which slots a selected part answers (`MHPacks_publishPartSlots` →
`$ModernHUD::PartSlots<handle>`). A part with no slot cannot be swapped.

**As the BASE pack** — yield every slot you do not own:
```
if(ModernHUDPack::ownsSlot($pref::HudSlot::weapon))
   <draw your weapon part>
else
   ModernHUD::hide("ModernHUD::<YourWeaponHandle>");
```
`ownsSlot("")` must be true, `ownsSlot("off")` false, and any `"<otherpack>/..."` false.
A part answering several slots yields if ANY of them is borrowed.

**As a PROVIDER** — the framework loads `Packs/<id>/components.cs` on its own, next to
whatever pack is the base (`ModernHUD::loadProvider`), and calls your component function
from `ModernHUD::drawSlot`. So `components.cs` must:

1. **Define only your own namespace** (`Ascend::`, `S26::`, `Overstep::`). Nothing in
   `ModernHUDPack::` — that namespace belongs to the base, and defining it would replace the
   base's functions. Do not call `ModernHUDPack::*` either: when borrowed it is the BASE's.
2. **Not depend on your `hud.cs`.** A borrowed part never runs your `init()` or `draw()`.
   Anything they set up per frame (palette, opacity/scale globals, lookup tables) goes in a
   `<NS>::compPrep()` the component calls — and your own `draw()` calls it too, so it exists
   once. Base-only work (pref swaps like `Vector::apply`) stays in `hud.cs`.
3. **Register each component on ONE line**, four quoted arguments — the client lists
   providers by reading this file as text, without executing it:
   ```
   ModernHUD::component("vector", "ctf", "ctf", "Vector::comp_ctf");
   ```
4. **Name your art relative to your own asset folder** (`Modules/HeEnHUD/Hring.png` for
   `Assets/Packs/overstep/Modules/...`). While your component draws, `$ModernHUD::DrawPack`
   names your pack and `glDrawImage` probes your folder first; `$ModernHUD::AssetRoot` points
   `.pft` markup at your fonts. Do not rely on your art being on the search path — only the
   base pack's is.
5. **Remember `$ModernHUD::PackId` is the BASE's id** while you are borrowed. Your part's
   position/size prefs are qualified by it on purpose (a part placed in Ascend and the same
   part placed in Basic are different layouts).

`hud.cs` execs `components.cs` right after `Framework.cs`, so the pack's own draw uses the
same code. **To split an existing pack:**
```
python tools/modernhud_components.py <id> --ns <NS> [--init] [--keep apply,defaults] [--move fnA,fnB]
```
It moves `function <NS>::*` and the `ModernHUDPack::draw_<partId>` wrappers, generates one
`<NS>::comp_<slot>` per slot, and refuses to write unless every function is still defined
exactly once, nothing left in `components.cs` touches `ModernHUDPack::`, nothing it calls
stayed behind in `hud.cs`, and braces balance.
★**A GENERATED pack (`"authoring": "generated"`) is split by the generator, not by hand.**★
Put the same options in its `pack.json` and regenerate:
```
"componentSplit": {"namespace": "basic", "splitInit": true}
```
(`move`, `keep`: comma-separated names; `renameGlobals`: true/false.)
`tools/modernhud_pack.py --generate` then writes `hud.cs` AND `components.cs`, the release
gate (`tools/modernhud_release_gate.py`) compares both, and `modernhud_convert.py` keeps the
key across a re-conversion. Splitting a generated pack with the tool alone fails the gate
and the next regenerate would undo it. ★It splits on `\n` only: these files hold
UTF-8 read as latin-1, and a `★` contains byte `0x85` (NEL), which Python's `splitlines()`
treats as a line break.★

### 11.4 Settings for one part

The optional 7th argument of `ModernHUD::setting` tags a setting to a part; the designer
lists it on that part's panel instead of under **Whole HUD**:
```
ModernHUD::setting("enum", "pref::Ascend::Health", "Health readout", "0",
   "Real (0-100)|0;Ascend scale (x15)|1", "", "ModernHUD::AscendVitals");
```
The tag is the part's handle name, or its tail (`"Vitals"`), or a 1.40 control
(`"chatDisplayHud"`, `"Minimap"`). A setting tagged to a part that is not on the preview
right now (switched off, or a mode the demo does not show) falls back to Whole HUD, so a
tag can never hide a setting. The K menu ignores the tag.

### 11.5 What the player can change, and where it is stored

| edit | pref (pack-qualified `<pack>::<handle>`) | read by |
|---|---|---|
| move | `$pref::hudPositions…` `"x y‖fx fy"` | `ModernHUD::handle` / `previewPos` |
| size (grip, wheel, slider) | `$pref::hudScale…` | `ModernHUD::part` → `glPartScale` |
| hide / opacity | `$pref::ModernHUD::PartHide…` / `PartAlpha…` | `ModernHUD::part` → `glPartStyle` |
| anchor | `$pref::ModernHUD::PartAnchor…` (a `ModernHUD::place` anchor name) | `ModernHUD::part` replaces your anchor, keeping your offsets |
| provider | `$pref::HudSlot::<slot>` | `ownsSlot` / `ModernHUD::drawSlot` |

All of them are captured by presets. `glPartStyle` hides and fades everything drawn
between one `ModernHUD::part` call and the next — so draw a part's content right after its
`part()` call, and do not draw another part's content before the next `part()`.
★Freehand drawing after a part inherits that part's style.★ Something that is not a part
(a frame around the native minimap, a centred cluster) drawn after the last `part()` is
hidden when THAT part is hidden. Call `glPartScale(0, 0, 1)` first — it resets the scale
and the style (Overstep's `drawMinimapFrame` draws right after Toasty and does this).
Something that deliberately never moves (Vector's reticle cluster) should say so with a
`PreviewNote`, or players will click it and wonder why nothing selects.

Resets: **Reset this part** clears that part's five prefs; **Reset all parts** and **Reset to
default** clear every `hudPositions` / `hudScale` / `PartHide` / `PartAlpha` / `PartAnchor` pref
qualified by your pack id — found by name, not by live handle, because the preview never
creates handles.

### 11.6 How to check a pack

1. Options → **08 CONFIGS/HUDS** at the MAIN MENU (no game yet): pick your pack; step
   through the four Demo scenarios. Anything empty should either fill in (§11.2) or say
   why (`PreviewNote`). The console prints one line per state:
   `[HUDSTAGE] ... scenario=<n> parts=<placed parts> draws=<draw calls> freehand=<draws before any part>`.
   `parts=0` with `draws>0` means nothing is selectable (§11.1); a large `freehand` is
   drawing that is not a part. The harness reads the stage as data from `$mcp::hudStage`
   (selected part, every part's rect/slots/provider, the preset list) while
   `$mcp::optionsRowsWant = 1`.
2. **Screen** at 1280x720 and 3840x2160: parts stay on screen and keep their corners.
3. Click each part: it selects, drags, resizes by grip and wheel, hides, fades, re-anchors.
   A part that cannot be clicked is not recorded (§11.1). Hide the LAST part your `draw()`
   places: nothing else may disappear with it (§11.5).
4. For each component, pick another pack as the base and choose yours in **Drawn by**;
   then do the reverse with your pack as the base. Your art and fonts must appear, and the
   base's own part for that slot must disappear.
5. Join a game: everything you changed in the designer is where you left it.
