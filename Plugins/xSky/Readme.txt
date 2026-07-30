xSky for Tribes 1.40/1.41
By Smokey

=======================================================================================================
About
=======================================================================================================
A plugin that renders a cubic skybox by modifying native Tribes rendering code.
This does NOT rely on an OpenGL overlay or other approach that may have additional performance overhead.

=======================================================================================================
Details
======================================================================================================

Skies are packaged and installed as individual .zip files to be installed to the following directory:

	- base/Entities/skies

For example:
	
	- base/Entities/skies/Cloudscape.zip

Configuration is set in 'config/Modules/xSky.acs.cs' with the following format:

	// Format is $xSky::Mission::<MapName> = "<SkyName>";
	$xSky::Mission::Default = "Cloudscape"; // Default sky
	$xSky::Mission::RaindanceLT = "Cloudscape";
	// ...
	// Add additional skies as needed