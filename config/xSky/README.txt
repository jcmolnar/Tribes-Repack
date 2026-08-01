=============================================================
 xSky - custom skyboxes  (config\xSky)
=============================================================

Every folder in here is one sky. Pick the active sky in game:
Options > Graphics > Sky (the < and > arrows cycle through them,
and the preview box shows each one).

A sky folder contains:
  <Name>_left/front/right/back/top/bottom.png   the six cube faces
  <Name>_sky.dml                                the material list
  <Name>_sky.png                                the menu preview
  <Name>_sky.cs                                 rotation/speed/haze defaults
  <Name>.png                                    (optional) the source panorama

-------------------------------------------------------------
 ADDING A NEW SKY - the easy way
-------------------------------------------------------------

You need one image: a 2:1 "equirectangular" panorama (the format
every 360-degree sky image site uses). Bigger is better; 2048x1024
minimum, 4096x2048 is great.

  1. Make a folder here named after your sky. Letters, digits and
     underscores only - no spaces, no dots. Example:  MySunset
  2. Put your panorama inside it, named after the folder:
         config\xSky\MySunset\MySunset.png     (.jpg works too)
  3. Open a command prompt in config\xSky and run:
         python build_xsky_pack.py MySunset
     (Needs Python with Pillow + numpy:  pip install pillow numpy)
  4. Start the game - your sky appears in Options > Graphics > Sky.
     Already in game? Open the console (~) and run:  xSky::rescan();

Run "python build_xsky_pack.py" with no name to build every folder
that has a panorama but no built sky yet.

-------------------------------------------------------------
 ADDING A READY-MADE SKY
-------------------------------------------------------------

Got a sky folder or .zip from another player? Drop the folder (or
the .zip, unchanged) straight into config\xSky. That is the whole
install.

-------------------------------------------------------------
 TUNING A SKY
-------------------------------------------------------------

Edit the sky's <Name>_sky.cs:
  $xSky::Settings::Rotation = 0;       starting rotation, degrees
  $xSky::Settings::Speed = 20;         slow drift, degrees per minute
  $xSky::Settings::Haze = "R G B";     distance-fog colour (0-255);
                                       delete the line to let the game
                                       sample it from the horizon

The sky obeys the mission's own sky unless you force it:
Options > Graphics > Sky > "Force On All Maps".
