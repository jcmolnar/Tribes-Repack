HEIGHTMAP FILES -- Tribes mission editor
========================================

Written by EXPORT HEIGHTMAP in the mission editor: Ctrl+P, type height.
HEIGHTMAP PANEL on that same list is the whole round trip on one card.

  <map>.r16         the terrain as a 16-bit little-endian raw, no header.
                    Open it as RAW: n x n, 16 bit, 1 channel, IBM PC byte
                    order, 0 header bytes. n is printed when you export.
  <map>.r16.txt     one line, "low high n": the world heights that 0 and
                    65535 stand for. Edit it to flatten or exaggerate.
  <map>_height.png  the same field as an 8-bit grey picture. White is high.

SHAPING A MAP WITH A PICTURE
  1. Edit <map>_height.png, or paint your own square grey picture here.
  2. Save it as grey PNG, 8-bit or 16-bit, not interlaced. Any square size
     works; it is resampled to the terrain.
  3. In the editor: Ctrl+P, type height, pick IMPORT HEIGHTMAP.
  4. Ctrl+Z undoes it. Ctrl+S saves the map.

WORTH KNOWING
  - Whichever of the .r16 and the .png you saved LAST is the one that gets
    imported. The console line says which it read and why.
  - Name one to be certain:  Ted::importHeightmap("mine.png");
  - A picture holds shades, not heights. The range comes from the .r16.txt
    if there is one, otherwise from the terrain you are standing on. Set it
    outright with  Ted::importHeightmap("mine.png", 0, 300);
  - The map tiles, so the last row and column are forced to match the first.
    A picture that is not seamless leaves a ridge where the map repeats. The
    import measures that step and prints it; SEAM BLEND on the panel, or
    $pref::meHmBlend = 8, smooths it over that many vertices.
  - 16-bit grey PNGs are read at full precision, the same as a .r16.
