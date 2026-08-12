config\Fonts\Sets  --  selectable font sets (Options -> Font Set)
=================================================================

Make a folder here and it appears in the Options "Font Set" dropdown.

    config\Fonts\Sets\Big and Bold\console.pft
    config\Fonts\Sets\Big and Bold\console.pft.000.png

The folder name is what shows in the dropdown.

A set OVERRIDES the HUD pack's own fonts. That is deliberate: if you don't like
the fonts a config pack ships with, pick a set and yours win instead.

You only need to include the fonts you want to change. Anything a set does not
contain falls back to the pack's font, then to the shared set in config\Fonts.

"Pack default" in the dropdown means "use no set" -- each HUD pack's own fonts
answer again.

FONT SCOPE V2: as the game-wide Font Set a folder here styles shell + HUD
(never the console). As a per-config Font Set it styles that config's HUD text
only. Ship each PFT's page sheets IN the set folder: pages are resolved from
the PFT's own directory only, never borrowed from another set or the root.
