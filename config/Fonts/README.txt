config\Fonts  --  ONE place for fonts, for the whole game
=========================================================

Drop a font here and it replaces that font EVERYWHERE: the shell, the HUD,
every config, every ModernHUD pack. This folder is checked FIRST, before
base\fonts.zip, before CustomConfigs\<config>\base\fonts.zip, and before any
ModernHUD pack's own fonts.

You do NOT have to use it.
-------------------------
Nothing is required to be here. If a font is not in this folder the game finds
it exactly where it always did. An empty folder changes nothing at all. This is
an ADDITIVE override, not a replacement for how fonts already load.

What to put here
----------------
A Tribes font is a .pft (the metrics) plus its page images, named after it:

    myfont.pft
    myfont.pft.000.png
    myfont.pft.001.png     (only if the font has more than one page)

Keep a .pft and its pages TOGETHER in this folder. The game deliberately
refuses to pair a .pft from here with page images from somewhere else -- that
mismatch produces wrong or missing glyphs, so it is treated as an error rather
than guessed at.

To replace an existing font, use its exact filename. For example, dropping
    console.pft + console.pft.000.png
here overrides the console font for every config in the game.

Why this folder exists
----------------------
The same font filename currently exists in many places with different contents
-- base\fonts.zip, base\rpgfonts.zip, each config's own fonts.zip, each HUD
pack's fonts.zip. Measured across this install: 148 distinct font names in 1572
copies, and 128 of those names (86%) exist in more than one place with
DIFFERENT bytes. Which one you actually got depended on load order.

This folder is the one deterministic answer: what is here wins.

Removing a font from this folder restores whatever the game used before.
