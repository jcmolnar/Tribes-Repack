# Authoring preview models — `models.json`

Adds an entry to the **Preview Model** list on Player Setup. For what that list is (and is not),
see `PLAYER_PREVIEW.md`.

```
config\UI\PlayerPreview\models.json
```

**The file is optional.** The five stock armors are compiled into the client. Deleting this file
loses nothing; a broken file loses only the entries it declared — the built-ins stay. The client
never falls back to an empty model list.

Validate before shipping:

```bash
python tools/player_preview_validate.py
```

It applies exactly the checks the client applies, and exits non-zero so it can gate a build. A
rejected entry is also named on the console at boot, but "my model just is not in the list" is a
symptom people report long before they read a log.

---

## Minimal known-good example

```json
{
  "format": 1,
  "models": [
    {
      "id": "my-scout",
      "label": "Scout Armor",
      "shape": "myscout.dts",
      "gender": "any",
      "skinMode": "authoredMaterials"
    }
  ]
}
```

That is the whole minimum. Everything else has a default.

---

## Schema

| Key | Type | Default | Notes |
|---|---|---|---|
| `id` | string | *required* | 1–30 chars of `A-Z a-z 0-9 _ -`. Stable: it is what gets saved. |
| `label` | string | *required* | What the player reads. |
| `shape` | string | *required* | A **bare** `.dts` resource name. No paths, no `..`. |
| `gender` | `male` \| `female` \| `any` | `any` | Which profile gender is offered this entry. |
| `skinMode` | see below | `legacySingleTexture` | How the skin is applied. |
| `skinSlot` | string | — | Required for `legacySingleTexture` and `materialRemap`. |
| `defaultAnimation` | string | `run` | Used when there is no saved preference. |
| `camera` | object | — | See below. |
| `allowRootMotion` | bool | `false` | Whether the `Free` in-place setting may let it travel. |

Unknown keys are **ignored, not rejected**. A manifest outlives any one build of the client, so a
key a newer client understands must not kill the entry on an older one. Malformed *values* still
fail, because those are genuine mistakes the author needs to hear about.

An entry whose `id` matches a built-in **replaces** it. That is how you retune the stock heavy
armor's camera without restating the other four.

---

## `skinMode` — the one that matters

### `legacySingleTexture`

Every textured material on the model is repointed at `<skin>.<skinSlot>.<ext>`.

This is the 1998 behaviour and it is correct for stock armor, which genuinely is one texture
wearing a skin name. **On anything else it is destructive** — a model with authored eyes, a visor,
insignia, or emissive panels loses all of them to the body skin. The validator warns when a
non-built-in model uses this mode.

### `authoredMaterials`

The model's materials are left exactly as authored. The player's skin selection does not affect
it. This is what a custom model almost always wants.

### `materialRemap`

Only materials whose map file already contains `.<skinSlot>.` are repointed. A multi-material
model can wear a skin on its body while keeping everything else.

---

## `camera`

All optional. Values outside the range are **clamped**, which means the preview will not match the
file — so the validator reports out-of-range as an error rather than a warning.

| Key | Range | Meaning |
|---|---|---|
| `yaw` | -100 … 100 | Starting turn, in **radians**, added to the default facing. |
| `pitch` | -1.5 … 1.5 | Starting tilt, in radians. Default `-0.25`. |
| `distanceScale` | 0.2 … 6.0 | Multiplies the auto-fit distance. `1.0` fits the model. |
| `targetZScale` | 0.0 … 1.0 | Where in the model's height the camera aims. `0.5` = middle. |
| `fov` | 5 … 120 | Field of view in **degrees**. Omit to use the preview default. |

Distance is **auto-fit from the model's actual bounding box**, so you usually need nothing here.
`distanceScale` exists for a model that reads better slightly closer or further out — a very tall
mech, or a model with a big empty bounding box.

---

## What is deliberately not possible

- **You cannot point at a `.dts` outside the mounted search path.** Separators, drive letters and
  `..` are refused. The `shape` is a resource name; the resource manager resolves it (including
  through the GLB replacement path, if that applies).
- **The list is an allow-list, not a scan.** The install holds thousands of `.dts` — weapons,
  vehicles, interiors, projectiles, debris, server content — and almost none of them are a player.
  Enumerating them all would make a menu of garbage, and would hand any file that names a shape a
  way into the preview.
- **A server cannot select a model.** Only ids already in this local registry can be selected, and
  the registry is only ever built from local files.

---

## Diagnosing an entry that will not show

```
playerPreviewList();       # what the registry actually holds, with built-in marked
playerPreviewReload();     # re-read both manifests without restarting
FGSkin::status(140003);    # what the preview control resolved
FGSkin::setModel(140003, "my-scout");
```

`setModel` is **atomic**: the candidate is loaded and proven before the live preview is given up.
If it fails you keep the model you had, and the console names the reason plus the model it fell
back to:

```
[PlayerPreview] ERROR stage=model reason="'my-scout' failed to load" lastWorkingModel="light-male"
```

Common causes, in order of likelihood:

1. The `.dts` is not on the search path — mount its volume first.
2. The entry was rejected at load. Run the validator; the boot console also names it.
3. The `gender` excludes the current profile, so the arrows skip it. Use `"any"` while testing.
