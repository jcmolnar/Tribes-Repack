# Native UI Theme Engine: Five Future Menu Systems

## Purpose

This document specifies five menu systems that cannot be authored faithfully with the
current SDF theme JSON format. They are intentionally ambitious, but each is designed as
an incremental extension of the existing native UI renderer rather than a replacement.

The current engine already supports:

- Eight fixed SDF shape rows.
- Per-skin palette, per-role colours, bevel, chamfer, skew, notch, rail, underline,
  charge bar, static scan modulation, segmented or bracket rings.
- One animated effect per style row: `matrix_rain` or `glitch_scan`.
- Hot reload through `uiThemeReload()`.

The concepts below must preserve existing themes unchanged. Missing or unsupported new
fields must fall back to the current renderer.

## Shared Implementation Rules

Primary code areas:

- `program/code/fearGuiSdf.cpp`: shaders, draw passes, role degradation, geometry.
- `program/inc/fearGuiSdf.h`: style and material definitions.
- `program/code/fearGuiTheme.cpp`: JSON parsing and runtime application.
- `program/code/fearGuiModernOptions.cpp`: player-facing controls.
- `tools/ui_theme_validate.py`: schema validation and semantic warnings.

Every extension must:

1. Remain optional and data-driven.
2. Keep old JSON files binary-compatible and visually unchanged.
3. Fail closed to the ordinary SDF widget when shaders or GPU features are unavailable.
4. Respect UI scale, safe area, role degradation, disabled state, and reduced-motion mode.
5. Batch draw calls by material where practical.
6. Expose a global quality setting: Off, Low, Full.
7. Keep the Options screen usable at 800x600 and 1920x1080.
8. Add validator coverage before shipping example themes.

---

## 1. Refractive Command Glass

### Player Experience

Panels behave like thin optical glass mounted over the game scene. The background behind a
panel is sampled, softened, slightly desaturated, and refracted near chamfered edges. Buttons
remain sharp and readable while panels gain depth without opaque slabs.

This would support a genuine cockpit, sensor-glass, or holographic-command menu rather than
simulating transparency with a dark fill.

### Why the Current Engine Cannot Do It

The current widget shader receives only widget-local geometry and colours. It cannot sample
the already-rendered scene behind the UI, blur neighbouring pixels, or apply a second material
layer independently of the SDF fill.

### Proposed JSON

```json
{
  "materials": {
    "commandGlass": {
      "type": "backdrop_glass",
      "blurRadius": 5.0,
      "refraction": 0.018,
      "saturation": 0.65,
      "tint": "#7DEBFF24",
      "noise": 0.025,
      "edgeGlow": 0.4
    }
  },
  "roles": {
    "panel": { "material": "commandGlass" }
  }
}
```

### Engine Work

- Capture the scene colour buffer once before shell UI rendering.
- Add a quarter-resolution ping-pong blur target for Low and a half-resolution target for Full.
- Add `SdfMaterialDef` and a material id to the widget draw command.
- Add a backdrop-glass shader pass clipped by the existing SDF distance.
- Reuse the SDF normal approximation to offset scene UVs near edges.
- Parse `materials` and role-level `material` references.
- Reject missing material references and out-of-range blur/refraction values.
- Add an opaque tinted fallback for software mode or failed framebuffer allocation.

### Performance Budget

- One scene copy per frame, not per panel.
- At most two separable blur passes.
- No more than one additional material draw per glass role batch.
- Low mode caps blur radius at 3 pixels and disables noise.

### Acceptance Criteria

- Multiple glass panels do not trigger multiple scene captures.
- Text contrast remains readable over bright snow, dark interiors, and sky.
- Chamfers clip both blur and refraction without rectangular leakage.
- Disabling the feature produces an opaque theme-consistent panel.
- Existing themes produce pixel-identical output when no material is declared.

---

## 2. Tactical Radial Command Wheel

### Player Experience

A true polar command menu opens around the cursor or screen centre. Wedges represent actions,
loadout groups, teams, or inventory categories. Selection follows angle and radius, supports
mouse and controller input, and confirms without requiring precise rectangular clicks.

This is useful in an FPS because it keeps choices spatially stable and minimizes cursor travel.

### Why the Current Engine Cannot Do It

Current controls are rectangular and laid out by existing GUI containers. The SDF shader can
decorate a rectangle but has no annular-sector primitive, polar hit testing, or focus navigation
for wedges.

### Proposed JSON

```json
{
  "radial": {
    "innerRadius": 56,
    "outerRadius": 154,
    "gapDegrees": 2.5,
    "startDegrees": -90,
    "deadZone": 44,
    "selectScale": 1.06,
    "labelRadius": 108,
    "confirmOnRelease": true
  }
}
```

### Engine Work

- Add an annular-sector SDF primitive with analytic antialiasing.
- Add `FearGuiRadialMenuCtrl` with polar layout, hit testing, focus, and action dispatch.
- Support 3 to 12 wedges and optional nested rings.
- Add controller stick angle selection with hysteresis around wedge boundaries.
- Add keyboard cycling and accessible linear fallback.
- Expose wedge, selected wedge, disabled wedge, centre, and submenu roles.
- Add Script console creation APIs so existing command data can feed the native control.
- Keep menu actions data-driven; the renderer must not own gameplay commands.

### Performance Budget

- One instanced draw batch for all wedges in a ring.
- No per-wedge framebuffer allocation.
- Polar text remains ordinary batched TTF text; labels do not rotate.

### Acceptance Criteria

- Every wedge has identical angular hit area after gaps.
- Selection does not flicker while the stick rests near a boundary.
- Mouse, keyboard, and controller can complete the same actions.
- At 800x600 the wheel remains inside the safe area and labels do not overlap.
- Reduced-motion mode disables wedge scale animation.

---

## 3. Layered Holographic Parallax Console

### Player Experience

Panels contain several independently moving depth layers: a chassis, a faint grid, tactical
marks, and a foreground edge. Mouse movement or controller focus produces a few pixels of
parallax. The result feels like a projected command volume, while remaining a flat readable UI.

### Why the Current Engine Cannot Do It

A current widget has one fill and one edge pass. Themes cannot define layers, masks, blend
modes, UV motion, or per-layer depth. The two existing effects are hardcoded alternatives
rather than composable passes.

### Proposed JSON

```json
{
  "materials": {
    "holoStack": {
      "type": "layer_stack",
      "layers": [
        { "source": "fill", "depth": 0.0 },
        { "source": "grid", "depth": 0.35, "scale": 18, "alpha": 0.12 },
        { "source": "scan", "depth": 0.55, "speed": 0.18, "alpha": 0.08 },
        { "source": "edge", "depth": 1.0, "bloom": 0.35 }
      ]
    }
  }
}
```

### Engine Work

- Replace the single effect enum with a small fixed material-pass array.
- Implement built-in procedural sources: fill, edge, grid, scan, noise, and telemetry ticks.
- Add blend mode, alpha, UV scale, UV speed, and depth to each pass.
- Compute one normalized parallax vector from cursor position or focused control.
- Limit layer count to four in the parser and validator.
- Compile shader variants from a bounded feature mask, not arbitrary theme shader text.
- Cache variants and use a known-good base shader on compilation failure.
- Add per-role material assignment and role degradation for compact rows.

### Performance Budget

- Maximum four layers and two animated sources per material.
- One shader variant per material feature mask, cached globally.
- Low mode flattens all depth to zero and drops layers after the second.

### Acceptance Criteria

- Parallax displacement never exceeds the widget clip area.
- Focus movement and cursor movement cannot combine into excessive offset.
- Compact list rows degrade to two layers without visual noise.
- Shader compile failure logs the material id and falls back to the base SDF renderer.
- A static screenshot remains legible without relying on animation.

---

## 4. Live Dataflow Operations Board

### Player Experience

Menu panels can display animated routes between real controls: server to mission, player to
team, inventory category to item, or objective to status. Thin lines travel through an
orthogonal graph, with packets moving along valid connections and selection highlighting a
specific route.

This turns the server browser, team setup, and inventory screens into coherent operations
boards rather than placing an unrelated animation inside every button.

### Why the Current Engine Cannot Do It

Existing effects are widget-local and know nothing about relationships between controls.
There is no retained overlay graph, routed path geometry, line joining, or theme-level binding
to control ids.

### Proposed JSON

```json
{
  "dataflow": {
    "lineWidth": 1.25,
    "cornerRadius": 5,
    "packetLength": 12,
    "packetSpeed": 42,
    "idleAlpha": 0.18,
    "activeAlpha": 0.8,
    "routing": "orthogonal"
  }
}
```

Connections should be supplied by the screen controller, not hardcoded in the theme:

```cpp
overlay->connect(sourceControl, targetControl, DataflowState::Active);
```

### Engine Work

- Add a retained `FearGuiDataflowOverlay` drawn after controls and before text tooltips.
- Add stable control-anchor queries for left, right, top, bottom, and centre.
- Implement bounded orthogonal routing around registered control rectangles.
- Tessellate rounded line joins into a dynamic vertex buffer.
- Animate packets by path distance using a shared time value.
- Add active, idle, warning, and disabled route colours.
- Re-route only after layout changes, not every frame.
- Provide a straight-line fallback if routing exceeds its node or time budget.

### Performance Budget

- Maximum 64 visible connections and 256 route segments.
- Maximum 0.5 ms routing budget after a layout change.
- One dynamic vertex buffer update per overlay per frame.
- Low mode removes packets and draws static routes.

### Acceptance Criteria

- Routes do not cross text or interactive control interiors when an alternate path exists.
- Resizing or UI scaling produces a stable re-route without one-frame stale geometry.
- Hidden controls remove their routes immediately.
- Screens with no registered graph incur no extra draw work.
- Reduced-motion mode keeps route highlighting but disables packet travel.

---

## 5. Diegetic 3D Inventory Carousel

### Player Experience

Inventory, armour, and weapon selection can present actual game models on a shallow 3D
carousel. The selected object is centred and inspectable; adjacent objects remain visible as
context. UI controls stay native SDF elements over the scene rather than becoming a separate
3D interaction system.

### Why the Current Engine Cannot Do It

The shell currently has isolated shape-view controls but no shared offscreen 3D stage,
carousel layout, depth-aware selection, model thumbnail cache, or theme-controlled lighting.

### Proposed JSON

```json
{
  "modelStage": {
    "background": "#05070C",
    "keyLight": "#B9E7FF",
    "fillLight": "#456080",
    "rimLight": "#FFB84D",
    "yawRange": 22,
    "sideScale": 0.72,
    "sideAlpha": 0.45,
    "transitionMs": 180
  }
}
```

### Engine Work

- Add one offscreen model stage shared by all carousel entries.
- Render the selected and nearest two neighbouring models into a composited target.
- Add deterministic fit-to-bounds using model radius and camera FOV.
- Add mouse drag inspection for the selected model and wheel/controller navigation.
- Cache model resources and generated thumbnails; release them when leaving the screen.
- Add theme-controlled three-light rig with clamped colour and intensity.
- Separate carousel data, gameplay selection, and rendering through a small adapter interface.
- Retain a 2D icon/list fallback for missing models or software rendering.

### Performance Budget

- Maximum three live models.
- Cap the offscreen target at 1024x1024 in Full and 512x512 in Low.
- Reuse depth and colour targets across screens.
- Pause model animation when the menu is obscured or unfocused.

### Acceptance Criteria

- Models of radically different sizes fit without clipping or disappearing.
- Switching items never loads synchronously on the render thread after initial cache fill.
- Missing models show the existing 2D item representation and remain selectable.
- Input focus cannot leak from carousel inspection into gameplay controls.
- Reduced-motion mode snaps carousel position while retaining manual model rotation.

---

## Recommended Implementation Order

1. Refractive Command Glass: establishes materials, role assignment, quality fallback, and
   scene capture infrastructure.
2. Layered Holographic Parallax Console: generalizes materials into bounded composable passes.
3. Tactical Radial Command Wheel: adds a new SDF primitive and interaction model.
4. Live Dataflow Operations Board: adds screen-level retained overlay geometry.
5. Diegetic 3D Inventory Carousel: highest integration cost and broadest resource-lifetime risk.

## Definition of Done

For each feature, Opus should deliver:

- Parser and validator changes with negative tests.
- A minimal reference JSON skin demonstrating only the new feature.
- Runtime fallback behavior tested with the feature disabled.
- Screenshots at 800x600 and 1920x1080.
- GPU timing in Off, Low, and Full quality.
- A short authoring-guide update describing supported values and known degradation.
- No visual change to the existing shipped theme set when new fields are absent.
