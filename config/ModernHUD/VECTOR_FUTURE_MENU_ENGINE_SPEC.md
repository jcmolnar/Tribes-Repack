# Vector and ModernHUD: Future ScriptGL Rendering Specification

## Purpose

This document defines five ambitious HUD and in-game menu styles that cannot be implemented
faithfully with the current ScriptGL drawing API. They are intended for Opus to implement as
engine capabilities that remain usable by Vector and every other ModernHUD pack.

This specification is separate from `config/UI/FUTURE_UI_THEME_ENGINE_FEATURES.md`:

- This file targets in-game HUDs and pack-owned menus drawn from TorqueScript through ScriptGL.
- The UI file targets the native SDF shell used by Options, the server browser, and other menus.

Nothing in this document is implemented yet.

## Current ScriptGL Boundary

Vector can currently compose useful HUDs from primitives such as:

- `glRectangle`
- `glGradientRect`
- `glAngledPolygon`
- ScriptGL text and image calls
- Mouse position and ordinary TorqueScript state

That is enough for opaque panels, rails, chamfers, grids, scanlines, colour themes, and custom
reticles. It is not enough for backdrop sampling, radial geometry, retained paths, real 3D
content, layered masks, or general animation timelines.

## Shared Requirements

Every new API must:

1. Be callable safely from TorqueScript.
2. Preserve the current ScriptGL state after returning.
3. Validate argument count, numeric ranges, object handles, and resource names.
4. Clip drawing to the active HUD or menu canvas.
5. Provide a cheap fallback when the GPU feature is unavailable.
6. Respect `$pref::Hud::Scale`, resolution changes, and safe-area positioning.
7. Avoid allocating GPU resources from a per-frame TorqueScript call.
8. Expose useful failures through the console without flooding it every frame.
9. Remain optional so existing packs render identically.
10. Include a small ScriptGL demonstration script and API documentation.

Primary implementation areas:

- `engine/SimGui/code/scriptGL.cpp`
- `engine/SimGui` ScriptGL console registration and state management
- the active OpenGL render path and framebuffer helpers
- `config/ModernHUD/Framework.cs` only where pack lifecycle cleanup is required

---

## 1. Holographic Parallax Console

### Experience

The Vector configuration panel becomes a projected command volume with several shallow depth
layers. A grid, telemetry marks, content, and luminous frame shift by different small amounts
as the cursor moves. Focus changes can send a soft highlight through the active layer.

This is still a practical FPS menu: the displacement is subtle, the text remains stationary,
and no interaction target moves away from its actual hit box.

### Required Engine Features

Add a bounded layer and mask system:

```text
glLayerBegin(layerId, x, y, w, h)
glLayerParallax(layerId, depth, maxOffset)
glLayerBlend(layerId, mode, opacity)
glLayerEnd(layerId)
glMaskBegin(x, y, w, h, shape, shapeArgs...)
glMaskEnd()
glLayerDraw(layerId, x, y)
glLayerRelease(layerId)
```

Supported blend modes should initially be:

- normal
- add
- multiply
- screen

`glMaskBegin` should support rectangle, chamfer, and polygon masks. Arbitrary shader text from
scripts is explicitly out of scope.

### Runtime Design

- A layer records ScriptGL draw commands into a reusable offscreen target.
- Static layers remain cached until explicitly invalidated.
- Dynamic content may be rebuilt once per frame.
- The renderer computes one normalized pointer vector per canvas.
- Parallax offsets are visual only; hit testing remains in unshifted menu coordinates.
- Text should normally be drawn after the parallax stack to preserve readability.

### ModernHUD Lifecycle

ModernHUD must release all layer handles owned by a pack when:

- the pack unloads;
- resolution changes;
- the connection closes;
- the renderer resets.

Add an ownership helper if raw integer handles cannot be cleaned up reliably:

```text
ModernHUD::ownGLResource(%handle);
```

### Fallback

When render targets are unavailable, `glLayerDraw` draws the recorded commands directly with
zero parallax and normal blending.

### Acceptance Criteria

- Four layers can be displayed without changing the menu's clickable rectangles.
- Static layers do not rebuild every frame.
- Text remains stable while decorative layers move.
- Unloading Vector releases every layer and render target.
- Existing ScriptGL state, colour, and scissor settings are restored afterward.
- The demonstration remains legible at 800x600 and 1920x1080.

---

## 2. Tactical Radial Command Wheel

### Experience

Vector can replace a long vertical menu with a radial wheel centred on the cursor or reticle.
Wedges can represent HUD pages, loadout groups, inventory actions, or quick commands. Selection
uses angle and distance, allowing a fast press-drag-release interaction suited to Tribes.

### Required Engine Features

Add radial geometry primitives and hit testing:

```text
glArc(cx, cy, radius, startDeg, endDeg, width)
glRing(cx, cy, innerRadius, outerRadius)
glWedge(cx, cy, innerRadius, outerRadius, startDeg, endDeg)
glWedgeGradient(cx, cy, innerRadius, outerRadius, startDeg, endDeg,
                innerR, innerG, innerB, innerA,
                outerR, outerG, outerB, outerA)
glPolarHitTest(mx, my, cx, cy, innerRadius, outerRadius, startDeg, endDeg)
```

The draw calls must use analytic or adaptively tessellated edges. A fixed low segment count will
look visibly broken at large radii.

### Interaction Helper

Provide one optional helper that returns the selected zero-based wedge:

```text
glRadialSelect(mx, my, cx, cy, innerRadius, outerRadius,
               itemCount, startDeg, gapDeg)
```

It returns `-1` inside the dead zone, outside the wheel, or inside a gap.

### Script Responsibilities

TorqueScript remains responsible for:

- labels and commands;
- disabled items;
- submenu state;
- confirm-on-click versus confirm-on-release;
- keyboard alternatives;
- sound.

The engine owns only geometry and reliable polar selection.

### Fallback

When radial primitives are unavailable, Vector keeps its existing rectangular menu. The pack
must test availability through:

```text
glHasFeature("radial")
```

### Acceptance Criteria

- Three through twelve wedges render with equal angular size.
- Gaps are excluded from hit testing.
- A wedge never changes selection because its hover decoration expands.
- Labels remain upright and do not overlap adjacent wedges at the supported item count.
- Selection is stable around the 0/360-degree boundary.
- The rectangular fallback remains fully usable.

---

## 3. Frosted Command Glass

### Experience

HUD menus and status panels can use real translucent glass: the world behind them is softened,
dimmed, and tinted, while the panel edge stays sharp. The effect should remain restrained enough
that a player can configure the HUD without losing awareness of combat behind the menu.

### Required Engine Features

Add a one-per-frame backdrop capture and clipped glass primitive:

```text
glBackdropCapture()
glFrostedRect(x, y, w, h, blurRadius, tintR, tintG, tintB, tintA,
              saturation, noise)
glFrostedChamfer(x, y, w, h, chamfer, cornerMask, blurRadius,
                 tintR, tintG, tintB, tintA, saturation, noise)
```

`glBackdropCapture()` must be idempotent within a frame. Calling it from several HUDs must not
copy the scene several times.

### Renderer Design

- Capture scene colour before ScriptGL HUD rendering.
- Use a shared downsampled texture.
- Apply separable blur at quarter resolution for the default quality.
- Clip the sampled backdrop with the same geometry used by the panel edge.
- Add noise procedurally rather than uploading a texture per panel.
- Do not include previously drawn HUDs in the sampled backdrop.

### Player Controls

Expose engine-level preferences:

```text
$pref::ScriptGL::Backdrop = "1";
$pref::ScriptGL::BackdropQuality = "1"; // 0 off, 1 low, 2 full
```

Pack authors may request a radius, but the global quality setting clamps it.

### Fallback

If capture or framebuffer setup fails, draw an opaque tinted rectangle or chamfer using the same
dimensions. Log the failure once.

### Acceptance Criteria

- Ten frosted panels still cause one scene capture.
- No rectangular blur leaks beyond chamfered corners.
- Text contrast is acceptable over sky, terrain, lava, and dark interiors.
- Disabling the preference immediately selects the opaque fallback.
- The fallback preserves panel size and menu interaction.
- Low quality stays within a 1 ms GPU budget on the project's minimum supported hardware.

---

## 4. Diegetic 3D Inventory Carousel

### Experience

An in-game inventory or equipment HUD can show actual armour, weapon, and item models in a
horizontal carousel. The selected model occupies the centre, adjacent models remain visible,
and the player can rotate the selected item without leaving the menu.

### Required Engine Features

Add a reusable ScriptGL model-view resource:

```text
glModelViewCreate(width, height)
glModelViewSetModel(handle, slot, modelPath, skinPath)
glModelViewSetTransform(handle, slot, x, y, z, yaw, pitch, roll, scale)
glModelViewSetCamera(handle, fov, distance, targetX, targetY, targetZ)
glModelViewSetLight(handle, index, dirX, dirY, dirZ, r, g, b, intensity)
glModelViewRender(handle)
glModelViewDraw(handle, x, y, w, h, opacity)
glModelViewClear(handle)
glModelViewRelease(handle)
```

Limit the first implementation to three visible model slots.

### Resource and Safety Rules

- Model loads must reuse the engine resource manager.
- A missing model or skin must return failure without crashing.
- Model bounds must be queryable for deterministic fit-to-view.
- Render targets must be shared or pooled by dimensions.
- Pack unload must release the handle and model references.
- Script code must not submit arbitrary world objects to the model view.

### Script Responsibilities

ModernHUD owns:

- carousel ordering;
- selection and gameplay command;
- transition timing;
- item names and statistics;
- drag-to-rotate state.

The engine owns:

- safe model loading;
- camera and lighting;
- render target;
- compositing.

### Fallback

Vector displays the existing item icon or text row when 3D views are disabled or a model cannot
load. Every item must remain selectable through the fallback.

### Acceptance Criteria

- Very small and very large models fit without clipping.
- At most three models render at once.
- Switching entries does not perform synchronous disk work every frame.
- Dragging the model cannot rotate the gameplay camera.
- Closing or unloading the menu releases all model-view resources.
- A missing asset shows a useful 2D fallback and console warning.

---

## 5. Live Dataflow Operations Board

### Experience

Vector can connect HUD modules and menu rows with routed luminous paths. A selected weapon can
link to its ammo and status block; a team objective can link to the relevant score; configuration
groups can show their data relationship. Small packets may travel along active paths.

Unlike a generic background animation, routes express actual relationships between HUD elements.

### Required Engine Features

Add retained path geometry:

```text
glPathCreate()
glPathMoveTo(handle, x, y)
glPathLineTo(handle, x, y)
glPathCurveTo(handle, cx1, cy1, cx2, cy2, x, y)
glPathOrthogonal(handle, startX, startY, endX, endY, bendRadius)
glPathStroke(handle, width, r, g, b, a)
glPathStrokeAnimated(handle, width, r, g, b, a,
                     packetLength, packetGap, speed, phase)
glPathBounds(handle)
glPathClear(handle)
glPathRelease(handle)
```

Paths must be retained because rebuilding curved or orthogonal geometry through TorqueScript on
every frame would waste CPU and create excessive console-call traffic.

### Optional Clipping

Paths should integrate with the layer and mask system from Concept 1:

```text
glPathClipRect(handle, x, y, w, h)
glPathExcludeRect(handle, x, y, w, h)
```

The initial implementation may support only one clip and sixteen exclusion rectangles per path.

### Renderer Design

- Tessellate curves and rounded bends only when path geometry changes.
- Store the result in a dynamic vertex buffer.
- Animate packet position in the shader using path distance.
- Use one shared time uniform.
- Keep path animation independent from the TorqueScript frame rate.

### Fallback

Without retained paths, Vector draws static one-pixel orthogonal lines with `glRectangle`.
Information must never depend solely on a moving packet.

### Acceptance Criteria

- A retained path incurs no geometry rebuild while only animation time changes.
- Curves and rounded corners remain smooth at UI scales from 75 to 200 percent.
- Exclusion rectangles prevent routes from crossing text and controls.
- Reduced-motion mode draws the route but disables packet travel.
- Unloading a pack releases all path buffers.
- Sixty-four visible paths remain within the agreed CPU and GPU budget.

---

## Proposed Capability Query

Add one stable feature query instead of forcing packs to infer support from command existence:

```text
glHasFeature("layers")
glHasFeature("radial")
glHasFeature("backdrop")
glHasFeature("modelview")
glHasFeature("paths")
```

Unknown feature names return `0`. Supported names return a positive integer API version, allowing
future revisions without inventing new queries.

## Recommended Implementation Order

1. Retained paths, because they require no scene capture or 3D resource integration.
2. Radial primitives and polar hit testing.
3. Layer recording, masks, and lifecycle ownership.
4. Frosted glass using the layer and framebuffer infrastructure.
5. Model-view resources and the inventory carousel.

## Definition of Done

For each capability, Opus should provide:

- C++ implementation with validated console bindings.
- A ScriptGL API reference entry.
- A self-contained test script under `config/ModernHUD/Tools`.
- A Vector demonstration that falls back cleanly.
- Resource-lifecycle tests covering pack unload and resolution change.
- Screenshots at 800x600 and 1920x1080.
- Frame-time and allocation measurements.
- Confirmation that existing ModernHUD packs render unchanged.
