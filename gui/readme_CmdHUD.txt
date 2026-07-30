!! CmdHUD 1.3a -> Commander/Inventory/Inv. Station LOOK-AROUND <- 5/9/99 !!

Original CmdHUD code, and -idea- by:
   Zear
   zear@planetstarsiege.com
   http:www.cetisp.com/~thammock/scripts/

InvHUD, CmdHUD rework, additional code by:
   Cowboy
   cowboy@planetstarsiege.com
   http:www.planetstarsiege.com/cowboy/
   ICQ# 31184463

CmdHUD 1.3a: got fed up, and got rid of the disappearing/reappearing Station
            inventory screen! (it's always on now), small caveat, but this
            way it doesn't interfere with Writer's fast_favorites.cs

     NOTE:  If you want to, you can even just install the 2 .gui files as
            mentioned, and not install the CmdHUD.cs script (if you like the
            way the GUIs look, but don't want the script installed...

            MAKE SURE you set $PrestoPref::InvCamera = false in pretoprefs.cs

CmdHUD 1.2: more bug fixes, added Teeni-Cam! This little HUD (inspired by
            CamHUD) displays a teeni playGui screen under the green Command
            panel when you have CmdHUD's full-size map displayed!

CmdHUD 1.1: lotsa bug-fixes, added $CmdHUD::DebugMode setting

Because of the way Writer's fast_favorites.cs works, sometimes you have to
press 'i' twice to view the inventory screen while at a station. This has to
do with the way he uses remoteval(2048, GuiMode). (maybe he can fix this?)

********************> NOW SUPPORTS RESIZE-ON-THE-FLY! <*********************

Well, I saw what Zear did with his CmdHUD, so I figured I'd do up an InvHUD
so people can have a much larger view around them at an inventory station!
Then, I looked at the CmdHUD and cleaned it up a little, and also made some
of the windows the same size/location between the 2 GUIs.

Next was trying to get it to resize! I got everything to resize, but much to
my dismay, every time you resized the window, all the playGui objects (clock
display, sensor ping, compass, etc) would do a 'snap to edge' and get fully
messed up. Presto had an idea: to place all of these 'wandering Huds' in a
separate frame, that wouldn't resize like the playGui. I had already tried
this but I didn't succeed.. but after a few days, I tried it again, because
I knew if I didn't do it, someone else might have and _they_ would get all
the credit... and lo! It worked! Wooooo baby! :)

Then.. people wanted to be able to use both the new small map AND the large
one from the original command.gui, I tried many different things.. i had it
so that it actually GuiPushDialog()'ed the old Command Gui screen onto the
MainWindow, but this was unreliable, not to mention _every_ time you wanted
to go back to that it had to be reloaded and rebuilt (even if it was the
last map displayed). So then I had the script create 2 FearGui::TSCommander
objects, one for each map size, in Simgui::Control objects, but the large
map couldn't resize, and if you made the window too small it would crash
Tribes (and although this worked fine for me and other people, i wanted it
to be able to FULLY resize on-the-fly).. So what I finally did is code into
the Gui 2 different Simgui::Control objects (one for each map, both opaque,
and the large one resizes) then I add the appropriate FearGui::TSCommander
maps into each window and rebuild.

The real trouble with that is that Tribes wouldn't give me the Object IDs
of the Simgui::Control objects (even though I named them, and knew where
they were) This was very frustrating.. I was ready to call it quits when
Presto suggested that since Simgui::Control objects are also sets (where
the objects it owns are the members), maybe I could get individual IDs of
members of a set whose ID I _could+ get (CommandGui). This worked! so, here
ya go!

NOTE: The Zoom/Center buttons and Zoom key do not work on the Commander Map!
      I don't know why, but they crash Tribes. if you know how to fix this,
      let me know ASAP! - but for now I disabled both the bottons and the
      'z' key in the Commander screen. Sorry!

What I did is take the 'center' button and change the binding to allow you
to change to Free Look / shut off the Commander Panel by clicking it...

Also, because the key bindings have changed, you might want to go through
your config.cs file and delete all the ones that were used by version 0.98
or older of CmdHUD.

Requires PrestoHUD, tested with 0.93! Also, this replaces Zear's CmdHUD.
Please do not run _both_ this CmdHUD and his old version concurrently!

(ok?)

I think I'm out of 'beta' here.. so Enjoy!

!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

Installation notes:

Unzip *.gui into the tribes\gui directory. If you don't have this directory,
create it! Unzip all other files into \config\cowboy directory and add line:
Include("cowboy\\CmdHUD.cs"); to your autoexec.cs file somewhere AFTER the
exec("presto\\install.cs"); line.

Also, set $PrestoPref::InvCamera = false in Presto's prestoprefs.cs!

!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

This was originally done by Zear, as was the idea. I want to take no credit
for the originality of it! I _did_ however do the Inventory screen... but 
only by first using his as a template...

(c) 1999 Zear/Cowboy, whatever... I hope you enjoy this, and all that i ask
is that you give Zear props for it.. He's the man... enjoy!

!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
