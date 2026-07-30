// xSky by Smokey
// xSky.dll required
// v0.7

// Skies (located in 'base/Entities/skies/<SkyName>.zip')
// Format is $xSky::Mission::<MapName> = "<SkyName>";
$xSky::Mission::Default = "Cloudscape"; // Default sky

$xSky::Mission::BroadsideLT = "";
$xSky::Mission::CanyonCrusadeDeluxeLT = "";
$xSky::Mission::DangerousCrossingLT = "";
$xSky::Mission::HildebrandLT = "";
$xSky::Mission::HillKingLT = "";
$xSky::Mission::LotusLT = "";
$xSky::Mission::RaindanceLT = "";
$xSky::Mission::RollercoasterLT = "";
$xSky::Mission::SnowblindLT = "";
$xSky::Mission::StonehengeLT = "";
// Add additional skies as needed


// Global Options
// Each individual sky sets specific Rotation, Speed, and Haze color parameters. Uncomment below to override these settings globally.
//
//$xSky::Rotation = 0;          // Rotation in degrees of the cubic skybox. Values from 0 to 360.
//$xSky::Speed = 0;             // Rotation speed of the cubic skybox. Values from 0 to 100 are reasonable (can be negative).
//$xSky::Haze = "255 255 255";  // Haze RGB (red, green, blue). Values can be between 0 and 255, inclusive.


// ==================================================================================================
// ==================================================================================================
// ==================================================================================================

function xSky::Init() {

    if (isFunction(xSky::xPrefs_Init)) {
        echoc(1, "xSky: Using xPrefs version of xSky.");
        return;
    }

    if ($xSky::_Loaded)
        return;

    $xSky::_Loaded = true;

    // Load sky volumes
    %file = File::findFirst("skies/*.zip");
    %i = 0;
    while (%file != "") {
        %id = newObject("Sky"~%i, SimVolume, %file);

        if (%id != 0) {
            %i++;
        }

        %file = File::findNext("skies/*.zip");
    }

    Event::Attach(eventMissionInfo, xSky::loadSky);

}

function xSky::loadSky() {
    if (!isObject(729)) // Object 729 is the Sky
        return;

    if ($ServerMission != "") {
        %skyName = "$xSky::Mission::" ~ $ServerMission;
        %skyName = *%skyName;

        if (%skyName == "") {
            %skyName = $xSky::Mission::Default;
        }

        if (%skyName != "") {
            %dml = %skyName ~ "_sky.dml";
            %file = File::findFirst(%dml);

            if (%file == "") {
                echoc(1, %dml ~ " is not loaded or does not exist.");
                xSky::Disable();
                xSky::setSpeed(0);
                xSky::resetHaze();
                return;
            }

            xSky::Enable();
            xSky::setDML(%file);
            xSky::resetHaze();

            %file = %skyName ~ "_sky.cs";
            if (File::findFirst(%file) != "") {
                deleteVariables("$xSky::Settings::*");
                %result = exec(%file);

                $xSky::Settings::Rotation = $xSky::Settings::Rotation != "" ? $xSky::Settings::Rotation : "0";
                $xSky::Settings::Speed = $xSky::Settings::Speed != "" ? $xSky::Settings::Speed : "0";

                xSky::setRotation($xSky::Settings::Rotation);
                xSky::setSpeed($xSky::Settings::Speed);

                if ($xSky::Settings::Haze != "" && String::explode($xSky::Settings::Haze, " ", "haze") == 3) {
                    %r = $haze[0];
                    %g = $haze[1];
                    %b = $haze[2];

                    xSky::setHaze(%r, %g, %b);
                }
            }

            if ($xSky::Rotation != "") {
                xSky::setRotation($xSky::Rotation);
            }

            if ($xSky::Speed != "") {
                xSky::setSpeed($xSky::Speed);
            }

            if ($xSky::Haze != "" && String::explode($xSky::Haze, " ", "haze") == 3) {
                %r = $haze[0];
                %g = $haze[1];
                %b = $haze[2];

                xSky::setHaze(%r, %g, %b);
            }
        } else {
            xSky::Disable();
            xSky::setSpeed(0);
            xSky::resetHaze();
        }
    }
}

xSky::Init();