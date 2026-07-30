# ModernHUD

ModernHUD is the native client's immediate-mode HUD framework. It formalizes
the existing ScriptGL path used by Kronos instead of creating persistent
SimGui controls.

Player instructions for choosing configurations, mixing parts, moving the HUD,
and managing complete presets are in [USER_GUIDE.md](USER_GUIDE.md).

Phase A adds two image commands:

```cs
glDrawImage(x, y, width, height, "path.png", 255);
glGetImageDimensions("path.png");
```

The resource may be PNG, GIF, TGA, BMP, or DIB. Animated GIFs advance through
the engine's normal animation registry. A non-positive width or height uses
the image's native dimension. Alpha accepts `0..1` or `0..255` and multiplies
the image's own alpha. The optional final argument is an RGB/RGBA hex tint or
`keyblack` for legacy black-key art.

`Framework.cs` supplies content-sized anchor placement and image-digit helpers.
A pack defines one typed draw entry point:

```cs
function ModernHUDPack::draw(%screenSize)
{
   ModernHUD::image("top-left", 24, 24, "Modules/frame.png", 255, "", %screenSize);
}
```

The pack sets `$ModernHUD::Enabled = true`; the native play-GUI hook calls it
once per frame. There are no persistent boxes, `fracPos`, scheduled render
updates, or string-built event dispatches.

`packs/Tribes_Overstep.phase_a.cs` is the first capability gate. It draws the
three Overstep status plates and image digits using content-relative layout,
plus a continuously fading minimap frame to verify per-frame image alpha.

Verification is required at 1920x1080 and 800x600 before this slice replaces
the retained controls.
