# UI Theme Framework — authoring guide

Everything needed to build a theme for the MODERN shell, and how the framework works underneath.
Written 2026-07-28. Authoritative sources, if this ever disagrees with the code:

| what | where |
|---|---|
| the renderer, styles, palette, roles | `program/code/fearGuiSdf.cpp`, `program/inc/fearGuiSdf.h` |
| the file format (the ONLY reader) | `program/code/fearGuiTheme.cpp` |
| the Options page rows | `program/code/fearGuiModernOptions.cpp` |
| accent / text / chassis colour maths | `program/code/fearGuiModernTheme.cpp` |
| format validator | `tools/ui_theme_validate.py` |
| design + traps | `re/modern_button_render_plan.md` |

---

## 1. What this framework is

Shell *widgets* — buttons, panels, sliders, fields, scroll regions — are drawn by **one GLSL
fragment shader** from a **signed distance field**, not from bitmap art. A widget's appearance is
therefore a small set of *numbers*, and a theme is a different set of numbers in a JSON file.

★**Coverage is not total, and this section used to claim it was.**★ "Every widget in the shell,
including list rows, is drawn by one shader" was disproved by source audit
(`re/ui_reskin_coverage_audit_2026-07-29.md`). Four rendering systems are actually in play, and a
theme reaches them to different depths:

| Surface | How a theme reaches it | Status |
|---|---|---|
| Buttons, panels, fields, sliders, scroll | SDF shader: palette + roles + styles | **fully themed** |
| Procedural page backdrop | palette `baseBottom` / `base` / panel role / `edge` | **themed** |
| Server browser list — header, rows, hover, dividers, glyphs | raw GL, remapped onto `roles.listrow` + palette | **themed chassis**, raw renderer |
| Master-server list (Options), Column Display (Address/Options) | shared list renderer, opted in | **themed** |
| Server Info — team headers, player rows, dividers, text | `roles.panel` / `roles.listrow` / `edge` / `text` | **themed** |
| Tabs (`FGTab`) — Address, TempChat | SDF `primary`/`button` + `text`/`textDim` | **themed** |
| Combo — chassis, arrow, label, value, popup rows | `SDF_ROLE_FIELD` + `edge` / `text` / `roles.listrow` | **themed** |
| `FGSimpleText` captions | palette `text` / `textDim` | **themed colour**, fixed typography |
| IRC channel + nickname lists | own `onRenderCell`, still stock | **not themed** |
| `FGTextList` (CreateServer, Recordings, Training, ircban) | PFT + palette index | **not themed** |
| `FGPaletteCtrl` swatches | stock | **content, not chrome** |
| Bitmap art (plates, frames, page backdrops) | optional global accent wash only | **weak** |
| Gameplay HUD, inventory, Commander | deliberately excluded | **by design** |

Typography is **fixed in code** (Segoe UI Semibold at fixed sizes). A theme can change text
*colour*, not the typeface. That is a current limitation, not an authoring gap.

Consequences worth understanding before authoring:

- **Fill and border can never disagree.** Both come from the same distance function. The shipped
  1998 art built "rounded" rects from three quads and stroked an arc over them; that class of bug
  is structurally impossible here.
- **Antialiasing is analytic** — one pixel of coverage at any size, any resolution, no MSAA.
- **No art to redraw.** A new look costs a text file, not a spritesheet.
- **It hot-reloads.** `uiThemeReload()` re-reads the file. No rebuild, no restart.

A theme can change colour, silhouette, bevel, per-element colour, and one animated effect. It
cannot add a new *shape* — those are the eight fixed style rows below — nor change layout.

---

## 2. Where files live

```
config\UI\Themes\<name>.json
```

Relative to the **game's** working directory — i.e. the folder holding `NativeTribes.exe`.

★A theme in the source tree is invisible in game.★ `Tribes Native Build\config\UI\Themes\` is
where files are authored; the game reads `C:\Dynamix\Tribes - Repack - 1.40 Assets\config\UI\Themes\`.
They must be copied over, exactly like the exe. This has already cost one debugging session.

The file's **basename is its id** (`matrix.json` → `uiTheme("matrix")`). `default` is reserved: it
restores the compiled-in table directly and never reads a file, so `default.json` is safe to edit
or delete — it exists only as authoring reference.

---

## 3. Console commands

| command | what it does |
|---|---|
| `uiTheme("name")` | Load and persist. Files itself into the skin or palette slot by the file's own `kind`. No argument returns the active skin. |
| `uiThemeReload()` | Re-apply the whole stack from disk. **The authoring loop:** edit JSON → run this → look. |
| `uiThemeList()` | List every theme found on disk. |
| `uiThemeReset()` | Reset the tuning sliders, KEEP the selected skin and palette, re-apply. "Put this skin back how its author intended." |
| `savePrefs()` | Force the `$pref::*` export to `config\ClientPrefs.cs`. |

All are registered from `configModules.cpp` at boot; `$pref::uiTheme`/`uiPalette` are applied there
too, so a saved choice survives a restart.

---

## 4. The two layers

★A skin and a colour preset are independent, and stored separately.★

```
$pref::uiTheme     the SKIN    — shapes AND colours   (default, winamp, matrix)
$pref::uiPalette   the PRESET  — colours only         (ocean, sunset, ember, royal, forest, mono)
```

Load order is skin first (full reset, its own palette), then the preset recolours **on top without
touching shapes**. A file declares which it is:

```json
"kind": "skin"        // shapes + colours. Full reset on load. Absent kind = skin.
"kind": "palette"     // colours only. MUST NOT carry "styles" or "select".
```

A `palette` load does a **colours-only reset**, which is what lets you click Ocean while Winamp is
active and keep Winamp's squared-off shapes.

★**Selecting a SKIN clears the preset slot.**★ The palette layer applies *after* the skin, so a
lingering preset would repaint every colour a skin authored — ten distinct skins then present as
nothing but shape differences, which is exactly what happened with `$pref::uiPalette = "forest"`
left selected. So the flow is:

| you click | you get |
|---|---|
| a skin | that skin's own shapes AND colours (preset slot cleared) |
| a preset, afterwards | the skin you are on, recoloured |
| another skin | its own colours again |

A skin ships a complete matched palette; a preset is a recolour you apply on top of one.

---

## 5. File format

```json
{
  "format": 1,
  "kind": "skin",
  "name": "Winamp Classic 2.x",
  "author": "you",
  "notes": ["free-form; ignored by the client"],

  "select": { "style": "plate" },

  "palette": {
    "base": "#2A2D34", "baseBottom": "#1B1D22",
    "primary": "#4E525D", "primaryBottom": "#32353E",
    "edge": "#787C88", "edgeShadow": "#101014",
    "disabled": "#1F2126", "disabledEdge": "#3A3D46"
  },

  "roles": {
    "panel":   { "base": "#1B1D22", "baseBottom": "#101014" },
    "listrow": { "edge": "#3FBEEF" }
  },

  "styles": {
    "chamfer": { "cham": 0, "corners": 0, "ring": 1.0, "bevel": 1 },
    "energised": { "effect": "matrix_rain", "animSpeed": 1.2, "leadBrightness": 2.0 }
  }
}
```

**Everything is optional.** A theme that sets one number keeps the defaults for the rest — the
loader resets to built-in first, then applies what the file says.

Colours are `"#RRGGBB"`, `"#RRGGBBAA"`, or `[r,g,b]` / `[r,g,b,a]` with channels **0–255**
(palette only; `roles` and `edgeOverride` take hex strings).

`select.style` forces `$pref::uiButtonStyle` — a skin shipping the shape it was designed around.
Not allowed on a `palette` kind: forcing a shape is a skin's job.

★**Effects belong to style rows. The theme's default animation is visible only when the selected
style itself declares that effect.**★ This is the single most common authoring mistake: an
`effect` on a row `select.style` does not choose is dormant in the theme's own default
presentation, and looks exactly like a broken shader. Four of the shipped skins had it. The
validator now warns:

```text
warning: animated effect exists only on non-selected style(s): energised;
selected style 'blade' will not animate
```

### 5.1 Palette slots

| key | what it colours |
|---|---|
| `base` / `baseBottom` | every widget body, as a vertical gradient |
| `primary` / `primaryBottom` | the accent-**filled** widget: selected list row, primary button |
| `edge` | the stroke. Also the colour `$pref::uiAccentHue` rotates, and the light half of a bevel |
| `edgeShadow` | the **dark** half of a 3D bevel (bottom/right) |
| `disabled` | greyed body |
| `disabledEdge` | greyed stroke |
| `text` | ★glyph colour★ — captions, list text, headings. Also drives the raw-GL server list |
| `textDim` | secondary / ghosted glyph colour |

`text` / `textDim` were added because text was previously **not part of the theme at all** — every
renderer hard-coded its own near-white, so a Pip-Boy amber or Blood Eagle skin could restyle each
widget shell while the words stayed modern blue-white. Omit them and those built-in colours stand.

A useful derivation, and the one the shipped skins use: **`text` = white pulled ~22% toward your
`edge`** (clearly tinted, still legible on a dark ground — legibility is the real constraint on
body text), and **`textDim` = that at ~62% brightness pulled a further ~18% toward `baseBottom`**,
so secondary text recedes into your own ground rather than toward a neutral grey.

### 5.2 Style rows — the eight fixed shapes

Ids are **fixed**; a theme restyles a row, it cannot rename or add one, because
`$pref::uiButtonStyle` and the Options picker key on these ids and a saved pref must keep resolving.

`chamfer` · `plate` · `bracket` · `keyed` · `blade` · `energised` · `segmented` · `rail`

| field | type | meaning |
|---|---|---|
| `cham` | px | corner chamfer size, at a 28px-tall reference widget |
| `corners` | bitmask | which corners are chamfered: `1`=TL `2`=TR `4`=BR `8`=BL (`5` = TL+BR, `15` = all) |
| `skew` | px | parallelogram shear (the `blade` look) |
| `ring` | px | stroke width. `0` = no stroke |
| `ringMode` | 0/1/2 | `0` full outline · `1` corner brackets · `2` segmented/dashed |
| `seg` | px | bracket arm length, or segment period when `ringMode` is 2 |
| `notch` | px | depth of a notch cut out of the top edge (`keyed`) |
| `rail` | 0–1 | inset accent rail along the chamfer plane |
| `under` | 0–1 | accent underline width, as a fraction of the widget |
| `bar` | px | leading charge bar (`energised`) |
| `scan` | 0–1 | **static** scanline modulation strength (~`0.03` subtle, `0.08` CRT). It does **not** move and does not imply an animated effect — describing it as a CRT sweep in `notes` promises something the renderer never does |
| `bevel` | 0–1 | `0` flat stroke · `1` full 3D: light top/left, shadow bottom/right. **Defaults to 1** |
| `edgeOverride` | hex | this style's own stroke colour, ignoring the palette |
| `effect` | `"none"` / `"matrix_rain"` / `"glitch_scan"` | animated in-widget pass. One per style row — the two are mutually exclusive |
| `animSpeed` | multiplier | effect speed (also scaled by `$pref::uiRainSpeed`) |
| `leadBrightness` | multiplier | brightness of the leading glyph of each rain column (rain only) |
| `effectDir` | `0` / `1` | which way `glitch_scan` runs. `0` (default) bands across with a tear that jumps; `1` bands down with a bright line sweeping **left to right**. Ignored by `matrix_rain` |

`edgeOverride` exists because one global accent cannot express a metallic chassis **and** a phosphor
LCD on the same screen. It is **not** run through hue rotation: an authored colour is a decision,
not a hue to rotate.

### 5.3 Roles — per-element colour

A **role** is the element class the renderer already draws by, so naming one colours that kind of
widget without touching the others, and without new plumbing in the widgets themselves.

`button` · `primary` · `panel` · `row` · `field` · `text` · `listrow`

Each takes `base`, `baseBottom`, `edge` (hex strings, all optional). Unset roles fall through to
the palette. Give only one of `base`/`baseBottom` and you get a flat fill from it rather than a
half-set gradient.

What maps to what, in practice: shell buttons and keybind buttons → `button`; the accent-filled
selection → `primary`; group frames, cards and dialog grounds → `panel`; a scroll region → `row`;
combos and text inputs → `field`; inventory/buy/roster rows → `listrow`; Options word-controls
(Yes/No, Plus/Minus) → `text`.

### 5.4 Role degradation — why a field can appear to do nothing

The style is a *shape language*; each role takes only the part that suits its size and job. This
runs **after** the file is read, so it silently overrides what a theme asks for:

| role | what it forces |
|---|---|
| `panel` | `ring × 0.6`, `cham × 1.6`, `bar` and `under` off |
| `row` | `cham`, `notch`, `skew`, `bar` off |
| `listrow` | as `row`, plus `seg`/`rail`/`under` off, and `ring` **only on hover** (`× 0.75`) |
| `field` | `bar`, `under` off |
| `text` | draws **nothing at rest**; hover/press draw an underline only |

So a chamfer set for a list row will not appear, by design — forty stacked rings read as a 1998
ruled table, which is what this framework exists to retire.

---

### 5.5 The two effects

Both are a second pass **inside** the same shader, clipped by the widget's own distance field, so
they follow every chamfer, notch and skew instead of being a quad pasted over the widget. Both share
`animSpeed`, `$pref::uiRainSpeed` and the `$pref::uiRain*` role gates — "where do effects go" is a
question about the role, so a second identical set of prefs would be noise.

| effect | looks like | notes |
|---|---|---|
| `matrix_rain` | falling code columns | per-cell 3×5 dot-matrix glyphs from a hash. `leadBrightness` brightens the head of each column |
| `glitch_scan` | tear bands + chromatic fringe + a bright line | for the cyberpunk / synthwave family. No texture to displace, so the tear is expressed in colour. `effectDir` picks the axis |

`text` roles never receive an effect. Hover accelerates 2.8×.

The two `glitch_scan` directions differ in **character**, not just rotation:

| `effectDir` | reads as |
|---|---|
| `0` (default) | a broadcast drop-out — bands across the widget, the bright tear jumping to a random line. Stuttery, nothing travels |
| `1` | a scanning beam — bands down the widget, a bright line sweeping left to right. The sweep is always on, because a travelling line that appears only 1.5% of the time never reads as travelling |

`cyberpunk` ships `effectDir: 1`; `synthwave` keeps the default. Both are hot-reloadable, so
flipping the value and running `uiThemeReload()` shows the other direction with no rebuild.

### 5.6 "This skin animates" vs "what I'm looking at animates"

Two different questions, and confusing them is what put animation sliders on a static shape:

| question | answer | used for |
|---|---|---|
| does any style row in this theme carry an effect? | `SdfThemeHasAnimatedStyle()` | describing a skin's capability |
| does the row `$pref::uiButtonStyle` selects carry one? | `SdfActiveStyleHasEffect()` | **gating the effect controls** |

A theme may ship `matrix_rain` on `energised` while the player is using `blade`. Nothing animates,
so the Shape Animation group is hidden. Switch UI Shape to `energised` and it reappears on the next
frame — Options rebuilds its rows every frame, so no reopen is needed.

### 5.7 `notes` are prose — the renderer never reads them

`notes` is documentation for humans. A colour named there does not exist; a behaviour described
there does not happen. Seven shipped skins advertised a second accent (brass, glacial white,
copper, quantum cyan, turquoise, purple, neon yellow) that lived **only** in `notes`, so the theme
rendered with one accent instead of two.

Every colour you name in `notes` must also appear in `palette`, a `roles` entry, or a style's
`edgeOverride`. The validator warns when it does not, and warns about behaviour words the engine
cannot deliver — `pulsing`, `stripes`, `animated scanline` / `CRT sweep`, and `degree` where `skew`
is measured in **pixels**.

## 6. Player controls (Options → Interface)

Tuning sliders are **unset** until moved, and unset means "the theme decides". Moving one takes
over that field globally; `uiThemeReset()` clears them.

| pref | range | effect |
|---|---|---|
| `uiButtonStyle` | style id | which of the eight shapes is active |
| `uiTheme` / `uiPalette` | name | skin / colour preset |
| `uiRing` | 0–80 | stroke width override, **px × 10** |
| `uiBevel` | 0–100 | bevel amount override |
| `uiChamfer` | 0–40 | chamfer px override |
| `uiScan` | 0–100 | scanline override (`/1000` internally) |
| `uiChassisTint` | 0–100 | how far the neutral chassis follows the accent hue. `0` = neutral |
| `uiAccentHue` / `Sat` / `Bri` | 0–360 / 0–100 / 0–200 | accent colour |
| `uiTextHue` / `Sat` / `Bri` | same | text colour |
| `uiArtTint` | 0–100 | accent wash over remaining stock bitmap art |
| `uiRainButtons` / `uiRainPanels` / `uiRainRows` | bool | where an effect is allowed. **The theme says the effect exists; the player says where it goes.** `text` never gets rain behind it |
| `uiRainSpeed` | 10–300 | multiplies the theme's `animSpeed`. Hover accelerates 2.8× |

These rows are grouped under **Shape Animation**, and appear **only** while the *selected UI Shape*
carries an effect (§5.6) — not merely while the theme has one somewhere. They sit last on the page,
because a conditional group anywhere else re-packs every row below it when it appears. The labels
are effect-neutral ("Animate buttons") since the same three prefs gate both rain and glitch-scan;
the pref keys stay `uiRain*` because they are already persisted in player configs.

Gates and knobs outside the Interface tab:

| pref | default | effect |
|---|---|---|
| `uiSdf` | on | `0` = fixed-function fallback everywhere; every caller keeps its 1998 path |
| `browserModern` | on | `0` = the full 1998 shell |
| `uiProcBackdrop` | on | generated animated backdrop instead of `Background1.png` |
| `uiMirrorLeftPanels` | on | mirrors panels lying entirely in the left half of the screen |
| `uiPackSettingsInOptions` | off | ModernHUD pack settings block on the Configs tab |

Diagnostics, all default-off: `uiSdfDiag` (resolved shader params per style/role/size/state),
`uiBoxDiag` (each panel's name/tag/rect), `uiThemeScanDiag` (theme folder scan + cwd),
plus `uiBtnDiag` / `uiPlateDiag` / `uiTextDiag` / `uiSurfDiag` / `uiRectDiag` / `uiInvDiag`.

---

## 7. Authoring workflow

1. Copy `default.json` (the built-in look, written out as reference) to `config\UI\Themes\mine.json`
   in the **game** folder.
2. Set `kind`, `name`.
3. Validate: `python tools/ui_theme_validate.py config/UI/Themes/mine.json`
4. In game: `uiTheme("mine")`, then edit and `uiThemeReload()` as often as you like.
5. Watch the console. The loader reports `applied: N group(s), M problem(s)` and names every bad
   key, unknown style, unknown role and malformed colour.

A theme added while the game is running needs a restart to appear in the Options **list** (the scan
is cached), but `uiTheme("name")` loads it immediately.

Validate before shipping. The client fails **closed** — a bad file leaves the built-in look
standing and says why — which is right at runtime but presents as "my skin did nothing". The
validator is the authoritative check and exits non-zero, so it can gate a build.

---

## 8. How it works, briefly

- **One `#version 120` fragment shader** for every widget. It reads `gl_Vertex` /
  `gl_MultiTexCoord0` / `gl_ModelViewProjectionMatrix`, so an immediate-mode quad inside
  `Control::onRender` inherits the engine's 2D pixel ortho with no VAO/VBO/FBO. Local coordinates
  arrive as **pixels from the widget centre**, so every distance is in pixels.
- **Entry points are resolved dynamically** via `wglGetProcAddress`, exactly as `rt.cpp` does —
  `opengl32.dll` exports only GL 1.1.
- **`SdfReady()` gates everything.** If the shader cannot compile or link, every caller keeps its
  original fixed-function path. The UI must never break on a driver.
- **The renderer exposes typed setters only** (`SdfStyleSet`, `SdfPaletteSet`, `SdfRoleFillSet`,
  `SdfRoleEdgeSet`). `fearGuiTheme.cpp` owns the file format, so the JSON has exactly **one reader**
  and the renderer never learns what a file looks like.
- **Parsing** uses jsmn (`third_party/jsmn.h`, `JSMN_STRICT`), extracted from the copy already
  inside `cgltf.h`.
- **Fails closed**: every load resets first. Which reset is correct depends on the file's own
  `kind`, so all failure paths reset before returning.
- **Mirroring** (`mirrorX`) swaps the quad's local x rather than rewriting the corner bitmask, so
  skew, notch and rail mirror along with the chamfer and no style needs a mirrored twin.
- **The bevel** finds which edge a pixel is on from the box distance itself — whichever axis is
  nearest its boundary, then the sign of the local coordinate there. Exact for rectangles,
  approximate on chamfered corners.
- **The rain** is a second pass *inside* the same shader, clipped by the widget's own distance
  field, so it follows every chamfer, notch and skew instead of being a rectangle pasted on top.
  Each cell gets a 3×5 dot-matrix pattern from a per-cell hash — that is what makes it read as
  characters without a glyph atlas. Time is a rebased float (`GetTickCount` modulo an hour) so
  precision never degrades over a long session.
- **`glShape(x,y,w,h[,role[,state]])`** exposes the same shape language to the **script** API, so
  ModernHUD packs can draw matching angled panels. ScriptGL has no polygon primitive otherwise.

---

## 9. Shipped themes

19 files. **Skins** (`kind:"skin"` — shapes and colours): `default` (built-in reference),
`winamp`, `matrix`, plus ten authored in one batch — `cyberpunk`, `starfortress`, `synthwave`,
`bloodeagle`, `starwolf`, `pipboy`, `solarpunk`, `quantumvoid`, `hazardmech`, `hyperlight`.
**Colour presets** (`kind:"palette"`): `ocean`, `sunset`, `ember`, `royal`, `forest`, `mono`.

★Six gaps were common to those ten as first written. All are fixed in the shipped files, and all
are now caught by the validator — but they are exactly what to check when authoring a new theme:★

1. **No `edgeShadow`.** Since `bevel` defaults to 1, every widget draws a bevel — and without this
   slot the shadow half falls back to the built-in `#101014`, which reads as a foreign material on a
   warm or violet theme. Derive it from your own darkest ground (≈45% of `baseBottom`).
2. **No `roles`.** Panels, list rows and fields then share one colour and the page reads flat. Give
   `panel` a darker ground than a widget (it sits behind content), `listrow` the widget colours, and
   `field` something recessed.
3. **No explicit `kind`.** Absent means skin, which was right here, but say it — the Options page
   files a theme by its declared kind, and a mislabelled palette will wipe shapes.
4. **No `bevel`.** Fine if you want the default raised look, but *say so per row*: four of these
   themes were designed flat and silently inherited the raised metallic edge. Declare `bevel` on
   every style row so the material is a decision rather than a default.
5. **An effect on a style the theme does not select** (§5.5–5.6) — dormant, and indistinguishable
   from a broken shader.
6. **Accents that exist only in `notes`** (§5.7) — seven of the ten advertised a second colour that
   was never a rendering field, so they drew with one accent.

Two of the ten animate by default: `cyberpunk` (`blade`, `glitch_scan`, `animSpeed` 1.25) and
`synthwave` (`keyed`, `glitch_scan`, 0.85). `matrix` (`energised`, `matrix_rain`) is the reference
implementation — copy its shape when adding an effect. The other eight are deliberately static;
`pipboy`'s and `quantumvoid`'s `scan` is fixed modulation, not a sweep.

## 9.1 Validating a theme

```bash
python tools/ui_theme_validate.py                 # every theme: structure + semantics
python tools/ui_theme_validate.py --self-test     # the validator's own fixtures
```

Errors mean the client will reject the file. **Warnings mean it will load and not look like it
claims** — a dormant effect, an implicit bevel, a note promising behaviour that does not exist, an
accent that is never drawn. Every one of those was a real defect in a file that passed the old
structural check.

## 10. Editing the shader

★Run `python tools/sdf_shader_audit.py` after ANY change to the shader strings.★ It reconstructs
each shader exactly as the driver receives it and reports a uniform used without a declaration, a
call that is neither a GLSL builtin nor defined locally, and brace imbalance.

This is not optional hygiene. A GLSL string is just text to the C++ compiler, so a missing
`uniform float uGlitch;` builds clean and then fails at runtime -- and a failed shader turns
`SdfReady()` false, which drops the ENTIRE shell to its 1998 fallback. One missing line looks
exactly like "themes are completely broken".

## 11. Traps

1. **★A theme must be DEPLOYED like the exe.★** Authoring in the source tree and testing in
   `C:\Dynamix\...` are different folders.
2. **A `palette` kind may not carry `styles` or `select`.** The client ignores such a block and
   says so; the validator rejects it.
3. **Role degradation overrides the file** (§5.4). If a field appears to do nothing, check there
   before assuming a bug.
4. **Style ids are fixed.** An unknown id is reported, not silently added.
5. **Authored colours bypass hue rotation.** `roles` and `edgeOverride` are decisions; only the
   palette `edge` follows `$pref::uiAccentHue`.
6. **A slider beats a theme.** Once a player moves `uiRing`/`uiBevel`/`uiChamfer`/`uiScan`, that
   field is theirs until `uiThemeReset()`.
7. **`uiChassisTint` at 0 does nothing by design** — the chassis is meant to stay neutral.
8. **Palette arrays are 0–255, not 0–1.** `[0.1, 0.5, 0.9]` is nearly black; the validator warns.
9. **Backslashes in C++ path literals must be doubled**, and in a string handed to `evaluate()`
   they need **four**. Written singly, the compiler only *warns* and the path silently fails —
   this cost a session when a theme-folder glob matched nothing with a correct cwd and the files
   plainly on disk.
10. **A check that passes either way is not a check.** Grepping the exe for
    `config\UI\Themes\*.json` matched the broken and the correct literal identically. A probe needs
    a field only one version can emit.
