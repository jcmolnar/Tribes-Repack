// GreyHound (Hunden) 2009 / 2010
// Set Map for bindings, this method will not clear the map, that allows us to ADD binds on the fly

function GameBinds::SetMapNoClearBinds( %sae )
{
    $GameBinds::CurrentMap = %sae;
    $GameBinds::CurrentMapHandle = GameBinds::GetActionMap2( %sae );
    //ActionMapList::clearBinds( $GameBinds::CurrentMapHandle );
}
 
//check if a certain binding is allowed
function GameBinds::isAllowed(%desc, %output)
{
        %map = String::tolower($GameBinds::CurrentMap);
        %desc = String::tolower(%desc);
 
        if($IgnoreBinds::ignore[%map, %desc])
        {
                if(%output)
                {
                        echoc(2, "The possibility to bind \"" ~ %desc ~ "\" was ignored because ...");
                        for(%i = 1; %i < $IgnoreBinds::ignore[%map, %desc, ignorecount]; %i++)
                        {
                                %descr = $IgnoreBinds::ignore[%map, %desc, %i, info];
                                echo(%descr);
                        }
                }
                return false;
        }
        return true;
}
/*      Check if the Action is not Ignored
        if it is: DONT ALLOW THE BIND
*/
function GameBinds::addBindActionFilter( %desc, %p0, %p1, %p2, %p3, %p4, %p5, %p6 ) before GameBinds::addBindAction{
        if(!GameBinds::isAllowed(%desc, true))
                halt;
}
/*Check if the Command is not Ignored*/
function GameBinds::addBindCommandFilter( %desc, %make, %break ) before GameBinds::addBindCommand {
        if(!GameBinds::isAllowed(%desc, true))
                halt;
}
/*
        Interface to Ignore Stuff
        That can be nice to avoid the bind map being clustered with deprecated things
        Example:
                A Script defines an advanced way to ski
                -> the default key can, and probably should be, ignored
*/
function GameBinds::ignore( %map, %desc, %info)
{
        %map = String::tolower(%map);
        %desc = String::tolower(%desc);
       
        if(!$IgnoreBinds::ignore[%map, %desc, ignorecount])
        {
                $IgnoreBinds::ignore[%map, %desc, ignorecount] = 1;
        }
        $IgnoreBinds::ignoreAction[%map, %desc] = true;
        $IgnoreBinds::ignore[%map, %desc] = true;
        if(%info != "")
        {
                $IgnoreBinds::ignore[%map, %desc, $IgnoreBinds::ignore[%map, %desc, ignorecount], info] = %info;
        }
        else
        {
                $IgnoreBinds::ignore[%map, %desc, $IgnoreBinds::ignore[%map, %desc, ignorecount], info] = "reason not specified";
 
        }
        $IgnoreBinds::ignore[%map, %desc, ignorecount]++;
 
}
//interface to re-allow a certain bind
function GameBinds::allow( %map, %desc, %info )
{
        %map = String::tolower(%map);
        %desc = String::tolower(%desc);
       
        $IgnoreBinds::ignore[%map, %desc] = false;
 
}

/*  Ignore some of the bind options that arent used anyway

    This file is intended to decrease the amount of totally unnecessary binds found in the bind menus
    GameBinds::ignore(%map, %description, %explanation);
*/
GameBinds::ignore( "playMap.sae", "Look Up", __FILE__ ~" thinks that bind is never used.");
GameBinds::ignore( "playMap.sae", "Look Down", __FILE__~" thinks that bind is never used.");
GameBinds::ignore( "playMap.sae", "Look Left", __FILE__~" thinks that bind is never used.");
GameBinds::ignore( "playMap.sae", "Look Right", __FILE__~" thinks that bind is never used.");
GameBinds::ignore( "playMap.sae", "Use Blaster", __FILE__ ~" thinks that bind is never used.");
GameBinds::ignore( "playMap.sae", "Use Plasma Gun", __FILE__~" thinks that bind is never used.");
GameBinds::ignore( "playMap.sae", "Use Laser Rifle", __FILE__~" thinks that bind is never used.");
GameBinds::ignore( "playMap.sae", "Use ELF Gun", __FILE__~" thinks that bind is never used.");
GameBinds::ignore( "playMap.sae", "Use Mortar", __FILE__~" thinks that bind is never used.");
GameBinds::ignore( "actionMap.sae", "Mission Edit Mode", __FILE__~" thinks that bind is never used.");

/* jumpjet bind example
GameBinds::ignore( "playMap.sae", "Jet", __FILE__ ~ " is better");
GameBinds::ignore( "playMap.sae", "Jump/Ski", __FILE__~ " is better");
//add the jumpjet bind
function JumpJet::addBindsToMenu() after GameBinds::Init
{
        GameBinds::SetMapNoClearBinds( "playMap.sae" );
        GameBinds::addBindCommand( "Hunden: Jump and Jet", "Jet::Start();", "Jet::Stop();" );
}
//add the jumping bind
function Jump::addBindsToMenu() after GameBinds::Init
{
        GameBinds::SetMapNoClearBinds( "playMap.sae" );
        GameBinds::addBindCommand( "Hunden: Skiing", "Jump::Start();", "Jump::Stop();" );
}
*/