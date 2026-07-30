xSky by Smokey

=======================================================================================================
Adding Additional Skies
=======================================================================================================

To add additional skies to xSky, package one individual sky into a .zip file and place it in:

    - base/Entities/skies/<name>.zip where <name> is the name of your sky (NO SPACES, use _ instead)

For example:

    - base/Entities/skies/Redstone.zip
    - base/Entities/skies/Darkfog.zip
    - base/Entities/skies/Clear_Day.zip

The zip file should have the following contents for one individual sky:

    <name>.zip:
        <name>_sky.dml (Required)
	<name>_left.png (Required)
        <name>_front.png (Required)
	<name>_right.png (Required)
        <name>_back.png (Required)
        <name>_top.png (Required)
        <name>_bottom.png (Required)
	<name>_sky.cs (Optional; Allows you to set custom sky render settings; See below)
        <name>_sky.png (Optional but strongly suggested; Width MUST be 275 pixels; See below)


=======================================================================================================
Sky Settings
=======================================================================================================
You can configure custom sky render settings if you provide a <name>_sky.cs file in the .zip.

The contents should include the below settings:

	$xSky::Settings::Rotation = 0;  	// Rotation in degrees of the cubic skybox. Values from 0 to 360.
	$xSky::Settings::Speed = 0;     	// Rotation speed of the cubic skybox. Values from 0 to 100 are reasonable (can be negative).
	$xSky::Settings::Haze = "255 255 255";	// Haze RGB (red, green, blue). Values can be between 0 and 255, inclusive. If not provided, the bottom left pixel of <name>_left.png is used.


=======================================================================================================
Preview Image
=======================================================================================================
You can add a preview image (<name>_sky.png) to the sky .zip that will display in Options/xPrefs (if used) when the sky is selected.
Including it is optional but strongly suggested.
The width MUST be 275 pixels and the height is suggested to be 140 pixels for optimal viewing but it can be different values.

Size Requirements:
    Width: 275 (Strict requirement)
    Height: 140 (Suggested height for optimal viewing)

