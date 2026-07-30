// localSoundAsync for Tribes 1.40/1.41
// Install to config/Core
// v0.1

// Adds async functionality to localSound()
// The native localSound() function cuts off previous sounds if they overlap
//
// =====================================================================================
// localSound(soundfile, [sync])
// =====================================================================================
// Replaces localSound(soundfile) with localSound(soundfile, [sync])
//
// Examples:
//      localSound("Flag1.ogg");            // Asynchronous - Sound will not be overridden by other calls to localSound()
//      localSound("Flag1.ogg", false);     // Asynchronous - Sound will not be overridden by other calls to localSound()
//      localSound("Flag1.ogg", true);      // Synchronous - Sound will be overridden by another call to localSound()
//
// Note:
//  If localSoundAsync.acs.cs is not installed, localSound(soundfile) will function synchronously
//  If you want your script to error if async is not installed, use localSoundAsync() instead
//  You can also check if async is installed in your script by running if(isFunction(localSoundAsync)) {}
//
// =====================================================================================
// localSoundAsync(soundfile, [delay])
// =====================================================================================
// Provides an async version of localSound() called localSoundAsync(soundfile, [delay])
//
// Example:
//       localSoundAsync("Flag1.ogg");      // Asynchronous - Sound will not be overridden by other calls to localSound() or localSoundAsync()
//       localSoundAsync("Flag1.ogg", 1);   // Asynchronous with 1 second delay before playing
//

function localSoundHook() before localSound {
    %soundfile = %argv[1];
    %sync = %argv[2];

    if (%soundfile == "") {
        echoc(0, "localSound( soundFile, [sync] )");
        halt "0";
    }

    // Native localSound supports sounds with no extension (for .ogg files)
    if (!String::ends(%soundfile, ".ogg") && !String::ends(%soundfile, ".wav")) {
        if (File::FindFirst(%soundfile~".ogg") != "") {
            %soundfile = %soundfile~".ogg";
        } else if (File::FindFirst(%soundfile~".wav") != "") {
            %soundfile = %soundfile~".wav"; // Add support for .wav
        } else {
            halt "0";
        }
    }

    if (File::FindFirst(%soundfile) == "")
        halt "0";

    if (%sync == true) {
        %argv[1] = %soundfile;
        return;
    }

    sfxAddPair(999999, IDPRF_2D, %soundfile);

    if ($Console::logBufferEnabled) {
        $Console::logBufferEnabled = 0;
        sfxPlay(999999);
        $Console::logBufferEnabled = 1;
    } else {
        sfxPlay(999999);
    }

    halt "0";
}

function localSoundAsync(%soundfile, %delay) {
    if (%soundfile == "") {
        echoc(0, "localSoundAsync( soundfile, [delay] )");
        return false;
    }

    if (%delay > 0) {
        schedule("localSound(\""~%soundfile~"\", false);", %delay);
    } else {
        localSound(%soundfile, false);
    }
}
