# The Player Setup character preview

The box on the Player Setup screen that shows your character. This document is for **players**.
If you want to add a model or a backdrop, read `MODEL_AUTHORING.md` and `BACKDROP_AUTHORING.md`
in this same folder.

---

## The one thing to know first

> **Preview Model changes what you are LOOKING at. It does not change what you SPAWN AS.**

Your in-game armor is chosen by the server and by your loadout. Nothing on this screen can
override that, by design — if it could, every server would have to defend against it. Setting the
preview to Heavy Armor and joining a game does not make you heavy.

What the profile *does* own, and what the preview reflects: your **name, gender, voice and skin**.
Those are real settings and they do travel to the server.

---

## Choosing a skin

The **Skin** drop-down lists every armor skin the game can find on your current search path. A
skin is a texture, so what it changes is how your character is painted, not their shape.

Skins are found as `<name>.larmor.png` (male) or `<name>.lfemale.png` (female), and `.gif` and
`.bmp` work too. If a skin ships in more than one format, PNG wins, then GIF, then BMP.

If the list is empty, the game says so on the console:

```
[PlayerPreview] ERROR stage=inventory reason="no .larmor/.lfemale skins found ..."
```

That means no skins are on the search path at all — usually a mod that has not finished loading,
or a broken install. It is not a preview bug.

---

## The preview controls

**Move the mouse over the preview** and a small browser appears over it. It disappears when you
move away, so the card is a character and not a control panel.

| Where | Control | What it does |
|---|---|---|
| Top strip | `<` `>` | Previous / next **Preview Model** |
| Bottom strip | `\|<` `>\|` | Previous / next animation |
| Bottom strip | `>` / `\|\|` | Play / pause |
| Bottom strip | `[3] run` | Which animation is playing, **by index** |
| Chip row | `1x` | Playback speed — 0.25x, 0.5x, 1x, 1.5x, 2x |
| Chip row | `Auto` | Loop mode — Auto, Loop, Once |
| Chip row | `Fixed` | In-place — `Fixed` runs on the spot, `Free` lets the animation travel |
| Chip row | `theme` | Backdrop |
| Chip row | `R` | Reset preview settings |
| Very bottom | thin bar | **Scrub** — click or drag to move through the animation |

Also:

- **Drag** on the character to turn it.
- **Right-drag** up and down to zoom.
- **Double-click** to jump to the next Preview Model.

On a small preview box the chip row, and then the transport row, are dropped rather than
squashed — the scrub bar and dragging always work.

### Why animations are numbered

Real Tribes models contain **more than one sequence with the same name**. `[1] run` and `[2] run`
are different clips. Showing the index is the only way to tell you which one is playing, and it
is why the preview selects animations by number internally.

### Loop mode

- **Auto** — do whatever the animation was authored to do. Looping clips loop; one-shot clips
  (deaths, most celebrations) play once and hold the last pose.
- **Loop** — repeat even a one-shot clip.
- **Once** — stop a looping clip at the end. Press Play to run it again.

---

## Backdrops

Built in: **Current Interface Theme** (follows whatever skin you picked in
Options → Interface Theme), **Transparent**, **Gradient**, **Grid**. A backdrop also carries a
lighting preset, because a bright backdrop behind an unlit model is not a preview of anything.

Image backdrops (PNG, or an animated GIF, which plays on its own) can be added — see
`BACKDROP_AUTHORING.md`.

---

## Where your settings live

These are saved with your other preferences and are **local to this machine**:

```
$pref::PlayerPreview::Model
$pref::PlayerPreview::AnimationName
$pref::PlayerPreview::AnimationOccurrence
$pref::PlayerPreview::Backdrop
$pref::PlayerPreview::Speed
$pref::PlayerPreview::Loop
$pref::PlayerPreview::InPlace
$pref::PlayerPreview::Controls      (set to 0 to hide the on-preview controls entirely)
```

The animation is remembered by **name plus occurrence**, never by raw index — index 7 means a
different clip in every model, so a saved index would quietly play the wrong animation as soon as
you changed models. If the next model has no clip by that name, it falls back to that model's
default.

### `R` (Reset preview settings) vs resetting your profile

`R` resets **only the things on this list**: model, animation, speed, loop, in-place, backdrop and
camera. It does **not** touch your name, gender, voice, skin, or any other player profile field.
There is no button on the preview that can change your profile.

---

## When something looks wrong

One command answers it:

```
FGSkin::status(140003)
```

(`140003` is the preview control's id, `IDCTG_PLAYER_TS`.) It prints the model, the resolved
shape, the skin and how it was applied, the backdrop and whether its image loaded, the animation
inventory with the playing one marked, the camera, and the material count.

The most useful line is this one:

```
[PlayerPreview] instance=NULL  <-- nothing will draw
```

which means no model is loaded at all — so the problem is upstream of anything on this screen.

For a running log of state changes rather than a snapshot:

```
$pref::playerPreviewDiag = 1;
```

It logs changes and failures only, never per frame.

To reload the model and backdrop manifests without restarting:

```
playerPreviewReload();
playerPreviewList();
```
