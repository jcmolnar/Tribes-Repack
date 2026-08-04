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
  pack.json     manifest -- identity, parts, anchors, asset map
  hud.cs        the script the client executes
```

Two files. Nothing else. A pack may reference shared art under
`config/ModernHUD/Assets/` and carried data layers under
`config/ModernHUD/Core/Data/`, but it owns only those two files — which is what
makes uninstalling one safe.

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
| `ModernHUDPack::stockHuds()` | on load + `eventGuiOpen` | show/hide the stock huds |
| `ModernHUDPack::detachRetained()` | on load | remove legacy containers this pack replaces (may be empty) |

Boot order at the bottom of `hud.cs`:

```
Event::Attach(eventGuiOpen, ModernHUDPack::onGuiOpen);
ModernHUDPack::prefs();
ModernHUDPack::stockHuds();
ModernHUDPack::init();
```

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
| `glPartScale(originX, originY, sx [, sy])` | scale subsequent draws about a point. Scale 1 = identity reset |

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
ModernHUD::setting(type, prefKey, label, default, spec, apply)
```

| arg | meaning |
|---|---|
| `type` | `"enum"` \| `"bool"` \| `"int"` |
| `prefKey` | a `$pref::` variable **without** the `$` — `"pref::Vector::Theme"` |
| `label` | shown in Options |
| `default` | seeded only if the pref has never been set |
| `spec` | enum: `"Label\|value;Label\|value;..."` · int: `"min\|max\|step"` · bool: unused |
| `apply` | console command run whenever the value CHANGES (may be `""`) |

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
