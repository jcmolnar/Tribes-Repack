// ---------------------------------------------------------------------------
// include.cs -- Version 1.5 -- April 7, 1999
// by Vis-DS.Writer (Lorne Laliberte, writer@videon.wave.ca)
//
// http://www.videon.wave.ca/~writer/tribes/
// ---------------------------------------------------------------------------


// A simple replacement for the exec() function.
//
// Inspired by (and mostly compatible with) the Presto Pack include.cs
//
// Use in your scripts wherever you want to call another script file,
// and the script will only be executed if there hasn't already been
// a script executed with the same name.
//
// Pass 'force' as the second, third or fourth argument to force execution
// of the script.
//
// Pass 'debug' as the second, third or fourth argument, or define the
// variable $Debug::echo as 'true' somewhere in your scripts, to see some
// debug information echoed to the console when the include function is called.
//
// Pass 'flat' as the second, third or fourth argument to ignore any path
// information, pretending all the script files were executed in one big
// directory, regardless of which directory they are actually in.
//
// Returns true if executed, false if not.
//
// Examples:
//
// include("myscript.cs");
// 
// ...will only execute myscript.cs if it hasn't already been executed
//
// include("myscript.cs", force);
//
// ...will execute myscript.cs regardless of what was run before
//
// include("myscript.cs", debug);
//
// ...will print a message to the console to show what happened
//
// include("mydir\\myscript.cs", flat);
//
// ...will only execute "mydir\\myscript.cs" if no other file named
//    "myscript.cs" was executed -- including "anotherdir\\myscript.cs"
//
// include("myscript.cs", debug, force, flat);
// include("myscript.cs", force, flat, debug); (etc. :)
//
// ...either of these will force execution of the script AND print
//    the debug info to the console AND only execute the script if
//    no other file named "myscript.cs" in any directory was executed.
//

function IncludeFile(%file) {
   return true;
}

function include(%file, %p1, %p2, %p3)
{
    %debug = %force = %executed = %flat = false; // not really necessary but I'm feeling paranoid :)
    
    // Check for debug argument or global $Debug::echo flag
    if( %p1 == "debug" || %p2 == "debug" || %p3 == "debug" || $Debug::echo || $PrestoPref::ScriptNoise)
        %debug = true;

    // Check for force argument
    if( %p1 == "force" || %p2 == "force" || %p3 == "force")
        %force = true;

    // Check for flat argument
    if( %p1 == "flat" || %p2 == "flat" || %p3 == "flat")
        %flat = true;

    // exec() will automatically append the .cs extension 
    // if it isn't present -- since we track the included
    // scripts by the name passed in %file, this could result
    // in a script being run twice (checking for "include"
    // won't find a match with "include.cs" and vice versa).

    // Automatically add the .cs extention if it's missing
    if( String::findSubStr(%file, ".cs") == -1)
        %file = %file @ ".cs";

    // Make %flatfile a copy of %file but without any path
    %flatfile = %file;
    while( (%backslashPos = String::findSubStr(%flatfile, "\\")) != -1 )
    {
        %flatfile = String::getSubStr(%flatfile, %backslashPos + 1, 1024);
    }

    if(%debug)
    {
        if(%flat)
            echo("include called with flat mode on -- paths will be ignored in previous inclusion test");
        else
            echo("include called with flat mode off -- paths will be used in previous inclusion test");
    }

    if( %force || (!%flat && !$Included::[%file]) || (%flat && !$Included::flat[%flatfile]) )
    {
        //Prevent recursive includes
        // Keep track of how many times file was included
        $Included::[%file]++; 
        $Included::flat[%flatfile]++;
	  %executed = exec(%file);// exec() returns false if it fails (could be a typo in file name :)
        if (!%executed)
           return error;

        if(%debug)
            {
                if(%flat)
                {
                    %includecount = $Included::flat[%flatfile];

                    // It bugs me to read "1 times" :)
                    %pluralize = "";
                    if(%includecount != 1)
                        %pluralize = "s";
                
                    echo("A file named " @ %flatfile @ " has been executed " @ %includecount @ " time" @ %pluralize);
                }
                else
                {
                    %includecount = $Included::[%file];
                
                    // It bugs me to read "1 times" :)
                    %pluralize = "";
                    if(%includecount != 1)
                        %pluralize = "s";

                    echo(%file @ " has been executed " @ %includecount @ " time" @ %pluralize);
                }
            }
    }
    else if(%debug)
    {
        if(%flat)
            echo(%flatfile @ " not executed -- a file by that name has been included already!");
        else
            echo(%file @ " not executed -- a file by that name has been included already!");
    }
    
    // Return true if executed, false if not
    return %executed;
}


// For compatibility with Presto's pack:
//
// Note that you can now add the argument "flat"
// to ignore any included paths when checking for
// previous includes.
//
function ReInclude(%file, %flat)
{
    Include(%file, force, debug, %flat);
}

//
// Check how many times a script has been included
//
// Pass 'flat' as the second argument to ignore any path information,
// pretending all the script files were executed in one big directory
// regardless of which directory they are actually in.
//
// Note -- use this if you want to test whether a script was included
//         without actually executing it
//
function included(%file, %flat)
{
    // Check for flat argument
    if(%flat == "flat")
        %flat = true;

    // Automatically add the .cs extention if it's missing
    if( String::findSubStr(%file, ".cs") == -1)
        %file = %file @ ".cs";

    // Make %flatfile a copy of %file but without any path
    %flatfile = %file;
    while( (%backslashPos = String::findSubStr(%flatfile, "\\")) != -1 )
    {
        %flatfile = String::getSubStr(%flatfile, %backslashPos + 1, 1024);
    }

    if(%flat)
    {
        return $Included::[%flatfile];
    }
    else
    {
        return $Included::[%file];
    }
}