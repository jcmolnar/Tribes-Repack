# Custom HUDs and presets

The native client can change HUD configurations without restarting Tribes.

## Choose a base HUD

1. Open **Options**.
2. Select **Configs**.
3. Choose a configuration under **Custom Configuration**.
4. Return to the game.

The selected configuration is activated when the game view returns. The choice
is saved and will also be used on the next launch.

Choosing a base configuration starts with that configuration's own HUD parts.
Parts borrowed from a previously selected configuration do not carry over.

## Mix individual HUD parts

The **HUD Parts** section lists replaceable portions such as health and energy,
weapons, the match clock, chat, minimap, and scoreboard.

- **Config Default** uses the active base configuration's implementation.
- **Off** disables that part.
- A named entry borrows that implementation from the displayed configuration.
- An asterisk indicates a part you selected manually.

Part changes apply live.

## Move and fade the HUD

Press **K** in the game to enter HUD-edit mode. Drag a HUD part or the minimap
to move it. Its position is saved for the active configuration.

Minimap opacity can be adjusted with:

```cs
$pref::miniMapOpacity = 128;
```

The range is `0` (invisible) through `255` (opaque). Configuration-authored
opacity remains in effect unless the player has explicitly overridden it.

## Save a complete preset

A preset is a complete HUD snapshot. It includes:

- the base configuration;
- every HUD-part selection;
- HUD positions;
- minimap position.

Under **HUD Presets**:

1. Select **Save current HUD as new preset...**
2. Enter a name.
3. Press **Enter** or choose **Save**.

Loading that preset later switches to its base configuration first and then
applies its parts and positions as one operation. The preset wins over whatever
configuration was active before it.

## Delete a preset

Choose **Delete Preset...**, select the preset, and confirm **Delete** in the
confirmation dialog. Deleting a preset never deletes an installed
configuration or its assets.

## Reset the HUD

- **Reset HUD positions only** restores authored positions while keeping the
  selected parts.
- **Reset HUD parts to config defaults** hands every part back to the active
  base configuration and restores its authored layout.

Resetting the live HUD never deletes saved presets.

## Troubleshooting

- If the options page says a configuration is **QUEUED**, return to the game to
  complete the hot reload.
- If a borrowed part is not wanted, select **Config Default** for that row.
- If the layout is unusable, use **Reset HUD positions only**.
- Enable `$pref::hudSlotDiag = 1;` only when collecting a diagnostic log; it is
  intentionally quiet during normal play.

