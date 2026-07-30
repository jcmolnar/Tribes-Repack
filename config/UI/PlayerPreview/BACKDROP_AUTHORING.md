# Authoring preview backdrops — `backdrops.json`

What the character stands in front of on Player Setup.

```
config\UI\PlayerPreview\backdrops.json
config\UI\PlayerPreview\Backdrops\        <- every image asset lives here. No exceptions.
```

**The file is optional.** `theme`, `transparent`, `gradient` and `grid` are compiled into the
client. A missing or broken file leaves those four working.

```bash
python tools/player_preview_validate.py
```

---

## Minimal known-good example

```json
{
  "format": 1,
  "backdrops": [
    {
      "id": "hangar",
      "label": "Hangar",
      "type": "image",
      "asset": "config/UI/PlayerPreview/Backdrops/hangar.png",
      "fit": "cover",
      "lighting": "studio"
    }
  ]
}
```

---

## Schema

| Key | Type | Default | Notes |
|---|---|---|---|
| `id` | string | *required* | 1–30 chars of `A-Z a-z 0-9 _ -`. Saved in the pref. |
| `label` | string | *required* | What the player reads. |
| `type` | `theme` \| `transparent` \| `gradient` \| `grid` \| `image` | `theme` | |
| `asset` | string | — | Required for `image`. Must be under `Backdrops\`. |
| `fit` | `cover` \| `contain` \| `stretch` | `cover` | `image` only. |
| `opacity` | 0.0 … 1.0 | `1.0` | |
| `tint` | `#RRGGBB` or `#RRGGBBAA` | white | Multiplied into the image. |
| `lighting` | `theme` \| `studio` \| `rim` \| `flat` | `theme` | Also accepts `{ "preset": "studio" }`. |

Unknown keys are ignored (forward compatibility); malformed values are errors.
An entry whose `id` matches a built-in replaces it — that is how you give the stock `grid` a
different lighting preset.

---

## The four generated types

| Type | What it draws |
|---|---|
| `theme` | The active Interface Theme's `panel` role, falling back to `base`/`baseBottom`. Follows the player's skin, so it is never the wrong colour for the rest of the screen. |
| `transparent` | Nothing. Whatever is behind Player Setup shows through. |
| `gradient` | A stronger vertical ramp than `theme` — light at the head, dark at the feet — so a dark model reads against it. |
| `grid` | The theme gradient plus a faint grid and a brighter horizon line at the model's feet, so the character has somewhere to stand. |

All of them recolour with the active theme, so a backdrop you author against one skin does not
become a foreign object under another.

---

## Image backdrops

**PNG, animated GIF and BMP** are all supported. An animated GIF **plays by itself** — the engine
registers animated bitmaps and ticks them once per presented frame, so there is nothing to drive
from here.

`fit`:

- **`cover`** — scale until the box is filled; the overflow is clipped. No letterboxing, some of
  the image is lost. Right for photographic or textural backdrops.
- **`contain`** — scale until the whole image fits. Nothing is lost; there may be bars.
- **`stretch`** — fill the box exactly, ignoring aspect ratio.

The preview box is small and roughly portrait. Author for that, or use `cover` and put nothing
important at the edges.

### The asset path rule

An asset must resolve under `config\UI\PlayerPreview\Backdrops\`. Absolute paths, drive letters
and `..` are refused, and the check runs with separators normalised so `/` and `\` both work.

This is not tidiness. A manifest that could name any path would be a file-read primitive with a
menu in front of it, and these files travel with mods.

### Failure behaviour

A missing or unreadable image **falls back to the theme backdrop** and says so once:

```
[PlayerPreview] ERROR stage=backdrop reason="...\hangar.png did not load" -- falling back to the theme backdrop
```

Once, not per frame — the failure is remembered, so one typo does not become a per-frame resource
lookup for as long as the screen is open.

---

## Lighting

Lighting travels **with the backdrop** rather than being a separate setting, because the two are
coupled: a bright backdrop behind an unlit dark model is not a successful preview.

| Preset | Ambient | Key | Use for |
|---|---|---|---|
| `theme` | 0.70 | 1.00 | The shipped look. Safe default. |
| `studio` | 0.45 | 1.25 | Bright or busy image backdrops — more contrast so the model does not flatten out. |
| `rim` | 0.28 | 1.35 | Dark backdrops. Deep shadows, hot edges. |
| `flat` | 1.05 | 0.00 | Ambient only, no key. Reads like a paper doll — useful for checking a skin texture without shading in the way. |

---

## Diagnosing

```
playerPreviewList();                        # ids, types, lighting, asset paths
playerPreviewReload();                      # re-read without restarting
FGSkin::setBackdrop(140003, "hangar");
FGSkin::status(140003);                     # includes  backdrop=... load=ok|FAILED|pending
```

`load=pending` means the backdrop is selected but the screen has not drawn yet — it loads on
first draw, not on selection.
